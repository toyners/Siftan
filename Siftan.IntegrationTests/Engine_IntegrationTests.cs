
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;
  using Siftan;

  [TestFixture]
  public class Engine_IntegrationTests
  {
    #region Methods
    [Test]
    public void Execute_SmallDelimitedFile_CorrectRecordsPassedToOutputMethods()
    {
      String testFilePath = null;

      try
      {
        testFilePath = Path.GetTempPath() + Path.GetRandomFileName();
        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.IntegrationTests.TestFile.csv", testFilePath);

        String matchedRecordTerm = null;
        Action<IStreamReader, Record> matchedRecordMethod = (reader, record) => 
        {
          matchedRecordTerm = record.Term;
        };

        String unmatchedRecordTerm = null;
        Action<IStreamReader, Record> unmatchedRecordMethod = (reader, record) =>
        {
          unmatchedRecordTerm = record.Term;
        };

        DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
        {
          Delimiter = "|",
          Qualifier = '\0',
          LineIDIndex = 0,
          HeaderID = "01",
          DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition("01", 3)
        };
        IRecordReader recordReader = new DelimitedRecordReader(recordDescriptor);

        new Engine().Execute(new[] { testFilePath }, new FileReaderFactory(), recordReader, new InListExpression(new[] { "12345" }), null);

        matchedRecordTerm.Should().Be("12345");
        unmatchedRecordTerm.Should().Be("54321");
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
