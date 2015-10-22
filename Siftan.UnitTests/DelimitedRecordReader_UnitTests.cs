
namespace Siftan.UnitTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using FluentAssertions;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Validation;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class DelimitedRecordReader_UnitTests
  {
    #region Methods
    [Test]
    public void ReadRecord_FileReaderIsClosed_ReturnsNull()
    {
      // Arrange
      IStreamReader mockReader = Substitute.For<IStreamReader>();
      mockReader.EndOfStream.Returns(true);

      DelimitedRecordReader reader = new DelimitedRecordReader(mockReader, new DelimitedRecordDescriptor());

      // Act and Assert
      reader.ReadRecord().Should().BeNull();
    }

    [Test]
    [TestCase("No delimiter")]
    [TestCase("H1,A,B,C", "L2,A,B,C", "L3,A,B,C")]
    public void ReadRecord_NoRecognisableRecord_ReturnsNull(params String[] lines)
    {
      // Arrange
      IStreamReader mockReader = this.CreateMockReaderInstance(lines);
      DelimitedRecordDescriptor descriptor = this.CreateDelimitedRecordDescriptor(",", '\0', 0, "H");
      DelimitedRecordReader reader = new DelimitedRecordReader(mockReader, descriptor);

      // Act
      Record record = reader.ReadRecord();

      // Assert
      record.Should().BeNull();
    }

    [Test]
    [TestCase(0, 7, "H,A,B,C")]
    [TestCase(0, 27, "H,A,B,C", "L1,A,B,C", "L2,A,B,C")]
    [TestCase(0, 29, "H,A,B,C", "L1,A,B,C", "L2,A,B,C", "H,D,E,F")]
    [TestCase(9, 16, "0,A,B,C", "H,A,B,C")]
    [TestCase(9, 26, "0,A,B,C", "H,A,B,C", "L1,A,B,C")]
    [TestCase(0, 17, "H,A,B,C", "L1,A,B,C")]
    [TestCase(0, 25, "H,A,B,C", "L1,A,B,C", ",D,E,F")] // Next record is missing the "H" line id
    public void ReadRecord_GoodRecordData_ReturnsRecord(Int64 recordStart, Int64 recordEnd, params String [] lines)
    {
      // Arrange
      IStreamReader mockReader = this.CreateMockReaderInstance(lines);
      DelimitedRecordDescriptor descriptor = this.CreateDelimitedRecordDescriptor(",", '\0', 0, "H");
      DelimitedRecordReader reader = new DelimitedRecordReader(mockReader, descriptor);

      // Act
      Record record = reader.ReadRecord();

      // Assert
      record.Should().NotBeNull();
      record.Start.Should().Be(recordStart);
      record.End.Should().Be(recordEnd);
    }

    [Test]
    [TestCase(0, 17, 0, "|H,1|,|A|,|B|,|C|")]
    [TestCase(0, 17, 1, "|A|,|H,1|,|B|,|C|")]
    [TestCase(0, 17, 2, "|A|,|B|,|H,1|,|C|")]
    [TestCase(0, 17, 3, "|A|,|C|,|B|,|H,1|")]
    public void ReadRecord_LineContainsQualifiers_ReturnsRecord(Int64 recordStart, Int64 recordEnd, Int32 lineIdIndex, String recordLine)
    {
      // Arrange
      IStreamReader mockReader = this.CreateMockReaderInstanceFromStrings(recordLine);
      DelimitedRecordDescriptor descriptor = this.CreateDelimitedRecordDescriptor(",", '|', (UInt32)lineIdIndex, "H,1");
      DelimitedRecordReader reader = new DelimitedRecordReader(mockReader, descriptor);

      // Act
      Record record = reader.ReadRecord();

      // Assert
      record.Should().NotBeNull();
      record.Start.Should().Be(recordStart);
      record.End.Should().Be(recordEnd);
    }

    [Test]
    public void CreateMockReaderInstance_OneLine_CreatesMockReader()
    {
      // Act
      IStreamReader mockReader = this.CreateMockReaderInstance(new[] { "First Line" });
      
      // Assert
      mockReader.EndOfStream.Should().Be(false);
      mockReader.Position.Should().Be(0);

      mockReader.ReadLine().Should().Be("First Line");
      mockReader.Position.Should().Be("First Line".Length);
      mockReader.EndOfStream.Should().Be(true);
    }

    [Test]
    public void CreateMockReaderInstance_ManyLines_CreatesMockReader()
    {
      // Act
      IStreamReader mockReader = this.CreateMockReaderInstance(new[] { "First Line", "Second Line", "Third Line" });

      // Assert
      mockReader.Position.Should().Be(0);
      mockReader.EndOfStream.Should().Be(false);

      mockReader.ReadLine().Should().Be("First Line");
      mockReader.Position.Should().Be("First Line\r\n".Length);
      mockReader.EndOfStream.Should().Be(false);

      mockReader.ReadLine().Should().Be("Second Line");
      mockReader.Position.Should().Be("First Line\r\nSecond Line\r\n".Length);
      mockReader.EndOfStream.Should().Be(false);

      mockReader.ReadLine().Should().Be("Third Line");
      mockReader.Position.Should().Be("First Line\r\nSecond Line\r\nThird Line".Length);
      mockReader.EndOfStream.Should().Be(true);
    }

    private IStreamReader CreateMockReaderInstanceFromStrings(params String[] fileLines)
    {
      return this.CreateMockReaderInstance(fileLines);
    }

    private IStreamReader CreateMockReaderInstance(String[] fileLines)
    {
      fileLines.VerifyThatArrayIsNotNullAndNotEmpty("Parameter 'fileLines' is null or empty.");

      IStreamReader mockReader = Substitute.For<IStreamReader>();

      Int32 position = 0;
      Int32 fileLineIndex = 0;
      mockReader.EndOfStream.Returns(x => 
      {
        if (fileLineIndex == fileLines.Length)
        {
          return true;
        }

        return false;
      });

      mockReader.Position.Returns(x => { return position; });

      mockReader.ReadLine().Returns(x =>
      {
        if (fileLineIndex == fileLines.Length)
        {
          return null;
        }

        position += fileLines[fileLineIndex].Length;
        String result = fileLines[fileLineIndex++];
        
        if (fileLineIndex < fileLines.Length)
        {
          // Was not the last line so add on the character count for '\r\n'
          position += 2;
        }

        return result;
      });

      return mockReader;
    }

    private DelimitedRecordDescriptor CreateDelimitedRecordDescriptor(String delimiter, Char qualifier, UInt32 lineIDIndex, String headerId)
    {
      return new DelimitedRecordDescriptor
      {
        Delimiter = delimiter,
        Qualifier = qualifier,
        LineIDIndex = lineIDIndex,
        HeaderID = headerId
      };
    }
    #endregion
  }
}
