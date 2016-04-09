
namespace Siftan.IntegrationTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using System.Text;
  using System.Text.RegularExpressions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;
  using Shouldly;

  [TestFixture]
  public class DelimitedRecordSource_IntegrationTests
  {
    private String workingDirectory;
    
    #region Methods
    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"DelimitedRecordSource_IntegrationTests\";
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      DirectoryOperations.EnsureDirectoryIsEmpty(this.workingDirectory);
    }

    [Test]
    public void HalfRecordInFile()
    {
      throw new NotImplementedException();
    }

    [Test]
    public void ReadRecordFromSingleRecordFile()
    {
      // Arrange
      String resourceFileName = "Siftan.IntegrationTests.Resources.SingleRecord.csv";
      String inputFilePath = this.workingDirectory + resourceFileName;
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFileName, inputFilePath);

      DelimitedRecordDescriptor recordDescriptor = CreateDelimitedDescriptor();

      // Act and Assert
      using (var source = new DelimitedRecordSource(recordDescriptor, inputFilePath))
      {
        source.GotRecord.ShouldBeTrue();
        source.MoveToNextRecord().ShouldBeFalse();
      }
    }

    [Test]
    public void ReadAllRecordsFromFile()
    {
      // Arrange
      String resourceFileName = "Siftan.IntegrationTests.Resources.TwoRecords.csv";
      String inputFilePath = this.workingDirectory + resourceFileName;
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFileName, inputFilePath);

      DelimitedRecordDescriptor recordDescriptor = CreateDelimitedDescriptor();

      // Act and Assert
      using (var source = new DelimitedRecordSource(recordDescriptor, inputFilePath))
      {
        source.GotRecord.ShouldBeTrue();
        source.MoveToNextRecord().ShouldBeTrue();
        source.MoveToNextRecord().ShouldBeFalse();
      }
    }

    [Test]
    public void GetRecordDataFromFile()
    {
      // Arrange
      String record1 = "01|Record1\r\n02|Record1\r\n03|Record1\r\n";
      String record2 = "01|Record2\r\n02|Record2";
      String inputFilePath = this.workingDirectory + "TwoRecords.csv";

      // Create the expected byte array for first record.
      Byte[] expectedData = new Byte[record1.Length];
      Int32 i = 0;
      foreach (Char c in record1)
      {
        expectedData[i++] = (Byte)c;
      }

      // Create content and write to file.
      var list = new List<String>(Regex.Split(record1 + record2, "\r\n"));
      File.WriteAllLines(inputFilePath, list.ToArray());

      DelimitedRecordDescriptor recordDescriptor = CreateDelimitedDescriptor();

      using (var source = new DelimitedRecordSource(recordDescriptor, inputFilePath))
      {
        // Act
        Byte[] buffer = new Byte[1024];
        Int64 bytesRead = source.GetRecordData(buffer);

        // Copy over the record data ahead of comparison to the expected record data
        Byte[] actualData = new byte[bytesRead];
        Array.Copy(buffer, actualData, bytesRead);

        // Assert
        bytesRead.ShouldBe(record1.Length);
        actualData.ShouldBe(expectedData);
      }
    }

    private DelimitedRecordDescriptor CreateDelimitedDescriptor()
    {
     return new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        LineIDIndex = 0,
        HeaderID = "01",
      };
    }
    #endregion 
  }
}
