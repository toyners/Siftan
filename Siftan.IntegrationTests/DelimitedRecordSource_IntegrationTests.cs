﻿
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using Jabberwocky.Toolkit.Assembly;
  using NSubstitute;
  using NUnit.Framework;
  using Shouldly;
  using TestSupport;

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
      TestDirectory.ClearDirectory(this.workingDirectory);
    }

    [Test]
    public void HalfRecordInFile()
    {
      throw new NotImplementedException();
    }

    [Test]
    public void Test()
    {
      // Arrange
      String resourceFileName = "Siftan.IntegrationTests.Resources.SingleFile.csv";
      String inputFilePath = this.workingDirectory + resourceFileName;
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFileName, inputFilePath);

      DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        LineIDIndex = 0,
        HeaderID = "01",
      };

      DelimitedRecordSource source = new DelimitedRecordSource(recordDescriptor, inputFilePath);

      // Act and Assert
      source.GotRecord.ShouldBeTrue();
      source.MoveToNextRecord().ShouldBeFalse();
      source.Close();
    }
    #endregion 
  }
}
