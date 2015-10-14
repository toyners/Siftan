
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using NUnit.Framework;

  [TestFixture]
  public class FileReader_IntegrationTests
  {
    #region Methods
    [Test]
    public void FileReader_OpeningExistingFile_FileIsOpened()
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.OneLine_NoTrailingTerminator.txt");

      // Act
      FileReader fileReader = new FileReader(filePath);

      // Assert
      fileReader.Position.Should().Be(0);
      fileReader.EndOfStream.Should().BeFalse();

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    [TestCase("Siftan.IntegrationTests.Resources.OneLine_NoTrailingTerminator.txt", 10)]
    [TestCase("Siftan.IntegrationTests.Resources.OneLine_StandardTrailingTerminator.txt", 12)]
    public void ReadLine_OneLineFileWithDifferentTrailingLineTerminators_FilePositionIsCorrect(String resourceName, Int32 expectedPosition)
    {
      // Arrange
      String filePath = this.CreateTestFile(resourceName);

      // Act
      FileReader fileReader = new FileReader(filePath);
      String result = fileReader.ReadLine();

      // Assert
      result.Should().Be("First Line");
      fileReader.Position.Should().Be(expectedPosition);
      fileReader.EndOfStream.Should().BeTrue();
      
      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    [TestCase("Siftan.IntegrationTests.Resources.OneLine_NoTrailingTerminator.txt", 10)]
    [TestCase("Siftan.IntegrationTests.Resources.OneLine_StandardTrailingTerminator.txt", 12)]
    public void ReadLine_TryingToReadPastEndOfFile_ReturnsNull(String resourceName, Int32 expectedPosition)
    {
      // Arrange
      String filePath = this.CreateTestFile(resourceName);

      // Act
      FileReader fileReader = new FileReader(filePath);
      String result = fileReader.ReadLine();
      result = fileReader.ReadLine();

      // Assert
      result.Should().BeNull();
      fileReader.Position.Should().Be(expectedPosition);
      fileReader.EndOfStream.Should().BeTrue();

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    [TestCase(1, "1st Line", 10)]
    [TestCase(2, "2nd Line", 20)]
    [TestCase(3, "3rd Line", 30)]
    public void ReadLine_ReadThreeLineFile_FilePositionIsCorrect(Int32 lineCount, String expectedLine, Int32 expectedLinePosition)
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");

      // Act
      FileReader fileReader = new FileReader(filePath);
      String result;
      do
      {
        result = fileReader.ReadLine();
      }
      while (--lineCount > 0);

      // Assert
      result.Should().Be(result);
      fileReader.Position.Should().Be(expectedLinePosition);

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    [TestCase(0, "1st Line")]
    [TestCase(8, null)]
    [TestCase(9, "\n2nd Line")]
    [TestCase(10, "2nd Line")]
    [TestCase(20, "3rd Line")]
    public void ReadLine_ChangePositionAndReadLine_ReturnLineIsCorrect(Int32 position, String expectedResult)
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");

      // Act
      FileReader fileReader = new FileReader(filePath);
      fileReader.Position = position;
      String result = fileReader.ReadLine();

      // Assert
      result.Should().Be(expectedResult);

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    public void ReadLine_LineTerminatorStraddlesBlocks_StateIsCorrect(Int32 bufferSize)
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");
      FileReader fileReader = new FileReader(filePath, 9);

      // Act
      String result = fileReader.ReadLine();

      // Assert
      result.Should().Be("1st Line");
      fileReader.Position.Should().Be(10);

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    public void Position_MovePositionInsideBuffer_StateIsCorrect()
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");
      FileReader fileReader = new FileReader(filePath);
      fileReader.ReadLine();

      // Act
      fileReader.Position = 1;

      // Assert
      fileReader.ReadLine().Should().Be("st Line");
      fileReader.Position.Should().Be(10);
      fileReader.EndOfStream.Should().BeFalse();

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [Test]
    public void Position_MovePositionGreaterThanBufferSize_StateIsCorrect()
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");
      FileReader fileReader = new FileReader(filePath, 3);
      fileReader.ReadLine();

      // Act
      fileReader.Position = 1;

      // Assert
      fileReader.ReadLine().Should().Be("st Line");
      fileReader.Position.Should().Be(10);
      fileReader.EndOfStream.Should().BeFalse();

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    [TestCase]
    public void Position_MovePositionOutsideBuffer_StateIsCorrect()
    {
      // Arrange
      String filePath = this.CreateTestFile("Siftan.IntegrationTests.Resources.ThreeLines_StandardTrailingTerminator.txt");
      FileReader fileReader = new FileReader(filePath, 6);
      fileReader.ReadLine();

      // Act
      fileReader.Position = 4;

      // Assert
      fileReader.ReadLine().Should().Be("Line");
      fileReader.Position.Should().Be(10);
      fileReader.EndOfStream.Should().BeFalse();

      // Cleanup
      this.Cleanup(fileReader, filePath);
    }

    private String CreateTestFile(String resourceName)
    {
      String filePath = Path.GetTempPath() + Path.GetRandomFileName();
      Assembly assembly = Assembly.GetExecutingAssembly();
      assembly.CopyEmbeddedResourceToFile(resourceName, filePath);
      return filePath;
    }

    private void Cleanup(FileReader fileReader, String filePath)
    {
      fileReader.Close();
      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }
    }
    #endregion
  }
}
