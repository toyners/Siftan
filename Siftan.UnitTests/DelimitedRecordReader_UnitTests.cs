
namespace Siftan.UnitTests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using NSubstitute;
  using NUnit.Framework;
  using FluentAssertions;

  [TestFixture]
  public class DelimitedRecordReader_UnitTests
  {
    [Test]
    public void ReadRecord_OneLineRecord_ReturnsRecord()
    {
      // Arrange
      IStreamReader mockReader = this.CreateMockReaderInstance(new [] { "First Line" });
      DelimitedRecordReader reader = new DelimitedRecordReader(mockReader);

      // Act
      IRecord record = reader.ReadRecord();

      // Assert
      record.Should().NotBeNull();
    }

    [Test]
    public void CreateMockReaderInstance_OneLine_CreatesMockReader()
    {
      // Act
      IStreamReader mockReader = this.CreateMockReaderInstance(new[] { "First Line" });
      
      // Assert
      mockReader.EndOfStream.Should().Be(false);
      mockReader.EndOfStream.Should().Be(true);
      mockReader.ReadLine().Should().Be("First Line");
    }

    [Test]
    public void CreateMockReaderInstance_ManyLines_CreatesMockReader()
    {
      // Act
      IStreamReader mockReader = this.CreateMockReaderInstance(new[] { "First Line", "Second Line", "Third Line" });

      // Assert
      mockReader.EndOfStream.Should().Be(false);
      mockReader.EndOfStream.Should().Be(false);
      mockReader.EndOfStream.Should().Be(false);
      mockReader.EndOfStream.Should().Be(true);
      mockReader.ReadLine().Should().Be("First Line");
      mockReader.ReadLine().Should().Be("Second Line");
      mockReader.ReadLine().Should().Be("Third Line");
    }

    [Test]
    public void CreateMockReaderInstance_FileLinesIsNull_ThrowsExpectedException()
    {
      // Act
      Action action = () => { this.CreateMockReaderInstance(null); };

      // Assert
      action.ShouldThrow<Exception>().WithMessage("Parameter 'fileLines' is null or empty.");
    }

    [Test]
    public void CreateMockReaderInstance_FileLinesIsEmpty_ThrowsExpectedException()
    {
      // Act
      Action action = () => { this.CreateMockReaderInstance(new String[] {}); };

      // Assert
      action.ShouldThrow<Exception>().WithMessage("Parameter 'fileLines' is null or empty.");
    }

    private IStreamReader CreateMockReaderInstance(String[] fileLines)
    {
      if (fileLines == null || fileLines.Length == 0)
      {
        throw new Exception("Parameter 'fileLines' is null or empty.");
      }

      IStreamReader mockReader = Substitute.For<IStreamReader>();
      Int32 eosIndex = 0;
      mockReader.EndOfStream.Returns(x => 
      {
        if (eosIndex == fileLines.Length)
          return true;

        eosIndex++;
        return false;
      });

      Int32 rlIndex = 0;
      mockReader.ReadLine().Returns(x =>
      {
        if (rlIndex == fileLines.Length)
          throw new Exception("Out of range in file lines array");

        return fileLines[rlIndex++];
      });

      return mockReader;
    }
  }
}
