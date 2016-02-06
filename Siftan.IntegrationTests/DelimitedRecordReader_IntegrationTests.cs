
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
        Int32 firstRecordEndPosition = "ï»¿01|Ben|Toynbee|12345|1.23  02|||12345||  03|||12345||  03|||12345||  05|||12345||  ".Length;
        Int32 secondRecordEndPosition = firstRecordEndPosition + "01|Sid|Sample|54321|1.23  02|||54321||  03|||54321||  05|||54321||".Length;

        // Arrange
        testFilePath = Path.GetTempPath() + Path.GetRandomFileName();
        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.IntegrationTests.TestFile.csv", testFilePath);

        DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
        {
          Delimiter = "|",
          Qualifier = '\0',
          LineIDIndex = 0,
          HeaderID = "01",
          Term = new DelimitedRecordDescriptor.TermDefinition("01", 3)
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
        firstRecord.End.Should().Be(firstRecordEndPosition);
        secondRecord.Start.Should().Be(firstRecordEndPosition);
        secondRecord.End.Should().Be(secondRecordEndPosition);
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
