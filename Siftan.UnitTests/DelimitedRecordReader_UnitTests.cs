
namespace Siftan.UnitTests
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Validation;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class DelimitedRecordReader_UnitTests
  {
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
      DelimitedRecordDescriptor descriptor = this.CreateDelimitedRecordDescriptor();
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
    [TestCase(0, 16, "H,A,B,C", "0,A,B,C")]
    public void ReadRecord_GoodRecordData_ReturnsRecord(Int64 recordStart, Int64 recordEnd, params String [] lines)
    {
      // Arrange
      IStreamReader mockReader = this.CreateMockReaderInstance(lines);
      DelimitedRecordDescriptor descriptor = this.CreateDelimitedRecordDescriptor();
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

    private DelimitedRecordDescriptor CreateDelimitedRecordDescriptor()
    {
      return new DelimitedRecordDescriptor
      {
        Delimiter = ",",
        Qualifier = "\"",
        LineTermIndex = 0,
        HeaderTerm = "H"
      };
    }
  }
}
