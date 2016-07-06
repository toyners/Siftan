
namespace Siftan.IntegrationTests
{
  using System;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class StatisticsManager_IntegrationTests
  {
    #region Methods
    [Test]
    public void StatisticsWrittenToJobLogCorrectly()
    {
      // Arrange
      var mockLogManager = Substitute.For<ILogManager>();

      const String InputFilePath = @"C:\InputFile.csv";
      const String MatchedOutputFilePath = @"C:\MatchedOutput.csv";
      const String UnmatchedOutputFilePath = @"C:\UnmatchedOutput.csv";

      var statisticsManager = new StatisticsManager();
      statisticsManager.RecordIsMatched(InputFilePath);
      statisticsManager.RecordIsUnmatched(InputFilePath);
      statisticsManager.RecordWrittenToOutputFile(MatchedOutputFilePath);
      statisticsManager.RecordWrittenToOutputFile(UnmatchedOutputFilePath);

      // Act
      statisticsManager.WriteToLog(mockLogManager);

      // Assert
      Received.InOrder(
        () =>
        {
          mockLogManager.WriteImportantMessageToJobLog("2 Record(s) processed.");
          mockLogManager.WriteImportantMessageToJobLog("1 Record(s) matched.");
          mockLogManager.WriteImportantMessageToJobLog("1 Record(s) not matched.");
          mockLogManager.WriteImportantMessageToJobLog(String.Format("2 Record(s) processed from input file {0}.", InputFilePath));
          mockLogManager.WriteImportantMessageToJobLog(String.Format("1 Record(s) matched from input file {0}.", InputFilePath));
          mockLogManager.WriteImportantMessageToJobLog(String.Format("1 Record(s) not matched from input file {0}.", InputFilePath));
          mockLogManager.WriteImportantMessageToJobLog(String.Format("1 Record(s) written to output file {0}.", MatchedOutputFilePath));
          mockLogManager.WriteImportantMessageToJobLog(String.Format("1 Record(s) written to output file {0}.", UnmatchedOutputFilePath));
        });
    }

    [Test]
    public void EmptyStatisticsWrittenToJobLogCorrectly()
    {
      // Arrange
      var mockLogManager = Substitute.For<ILogManager>();
      var statisticsManager = new StatisticsManager();

      // Act
      statisticsManager.WriteToLog(mockLogManager);

      // Assert
      mockLogManager.Received(1).WriteImportantMessageToJobLog(Arg.Any<String>());
      mockLogManager.Received().WriteImportantMessageToJobLog("0 Record(s) processed.");
    }
    #endregion
  }
}
