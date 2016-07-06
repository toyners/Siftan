
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;
  using TestSupport;

  [TestFixture]
  public class DelimitedRecordReader_IntegrationTests
  {
    private String workingDirectory;

    #region Methods
    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"DelimitedRecordReader_IntegrationTests\";
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      DirectoryOperations.EnsureDirectoryIsEmpty(this.workingDirectory);
    }

    [Test]
    public void ReadRecord_TwoRecordsInFile_ReturnsTwoRecordObjects()
    {
      // Arrange
      String resourceFileName = "Siftan.IntegrationTests.Resources.TestFile.csv";
      String inputFilePath = this.workingDirectory + resourceFileName;
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFileName, inputFilePath);

      var recordDescriptor = new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        LineIDIndex = 0,
        HeaderID = "01",
        Term = new DelimitedRecordDescriptor.TermDefinition("01", 3)
      };

      var reader = new DelimitedRecordReader(recordDescriptor);

      var fileReader = new FileReader(inputFilePath);

      // Act
      var firstRecord = reader.ReadRecord(fileReader);
      Int64 firstRecordEndPosition = fileReader.Position;
      var secondRecord = reader.ReadRecord(fileReader);
      Int64 secondRecordEndPosition = fileReader.Position;

      fileReader.Close();

      // Assert
      firstRecord.Should().NotBeNull();
      secondRecord.Should().NotBeNull();
      firstRecord.Should().NotBeSameAs(secondRecord);

      firstRecord.Start.Should().Be(0);
      firstRecord.End.Should().Be(firstRecordEndPosition);
      secondRecord.Start.Should().Be(firstRecordEndPosition);
      secondRecord.End.Should().Be(secondRecordEndPosition);
    }
    #endregion
  }
}
