
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
      TestDirectory.ClearDirectory(this.workingDirectory);
    }

    [Test]
    public void ReadRecord_TwoRecordsInFile_ReturnsTwoRecordObjects()
    {
      // Arrange
      String resourceFileName = "Siftan.IntegrationTests.Resources.TestFile.csv";
      String inputFilePath = this.workingDirectory + resourceFileName;
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFileName, inputFilePath);

      DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        LineIDIndex = 0,
        HeaderID = "01",
        Term = new DelimitedRecordDescriptor.TermDefinition("01", 3)
      };

      DelimitedRecordReader reader = new DelimitedRecordReader(recordDescriptor);

      FileReader fileReader = new FileReader(inputFilePath);

      // Act
      Record firstRecord = reader.ReadRecord(fileReader);
      Int64 firstRecordEndPosition = fileReader.Position;
      Record secondRecord = reader.ReadRecord(fileReader);
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
