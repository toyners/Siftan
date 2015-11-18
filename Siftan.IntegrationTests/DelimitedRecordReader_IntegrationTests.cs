
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;

  [TestFixture]
  public class DelimitedRecordReader_IntegrationTests
  {
    #region Methods
    [Test]
    public void ReadRecord_TwoRecordsInFile_ReturnsTwoRecordObjects()
    {
      String testFilePath = null;
      try
      {
        // Arrange
        testFilePath = Path.GetTempPath() + Path.GetRandomFileName();
        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.IntegrationTests.TestFile.csv", testFilePath);

        DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
        {
          Delimiter = "|",
          Qualifier = '\0',
          LineIDIndex = 0,
          HeaderID = "01",
          DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition("01", 3)
        };

        DelimitedRecordReader reader = new DelimitedRecordReader(recordDescriptor);

        FileReader fileReader = new FileReader(testFilePath);

        // Act
        Record firstRecord = reader.ReadRecord(fileReader);
        Record secondRecord = reader.ReadRecord(fileReader);

        fileReader.Close();

        // Assert
        firstRecord.Should().NotBeNull();
        secondRecord.Should().NotBeNull();
        firstRecord.Should().NotBeSameAs(secondRecord);

        firstRecord.Start.Should().Be(0);
        firstRecord.End.Should().Be(83);
        secondRecord.Start.Should().Be(83);
        secondRecord.End.Should().Be(152);
      }
      finally
      {
        if (testFilePath != null && File.Exists(testFilePath))
        {
          File.Delete(testFilePath);
        }
      }
    }
    #endregion
  }
}
