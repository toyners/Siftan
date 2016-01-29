
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
      ILogManager mockLogManager = Substitute.For<ILogManager>();

      const String InputFilePath = @"C:\InputFile.csv";
      const String MatchedOutputFilePath = @"C:\MatchedOutput.csv";
      const String UnmatchedOutputFilePath = @"C:\UnmatchedOutput.csv";

      StatisticsManager statisticsManager = new StatisticsManager();
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
          mockLogManager.WriteMessageToJobLog("2 Record(s) processed.");
          mockLogManager.WriteMessageToJobLog("1 Record(s) matched.");
          mockLogManager.WriteMessageToJobLog("1 Record(s) not matched.");
          mockLogManager.WriteMessageToJobLog(String.Format("2 Record(s) processed from input file {0}.", InputFilePath));
          mockLogManager.WriteMessageToJobLog(String.Format("1 Record(s) matched from input file {0}.", InputFilePath));
          mockLogManager.WriteMessageToJobLog(String.Format("1 Record(s) not matched from input file {0}.", InputFilePath));
          mockLogManager.WriteMessageToJobLog(String.Format("1 Record(s) written to output file {0}.", MatchedOutputFilePath));
          mockLogManager.WriteMessageToJobLog(String.Format("1 Record(s) written to output file {0}.", UnmatchedOutputFilePath));
        });
    }

    [Test]
    public void EmptyStatisticsWrittenToJobLogCorrectly()
    {
      // Arrange
      ILogManager mockLogManager = Substitute.For<ILogManager>();
      StatisticsManager statisticsManager = new StatisticsManager();

      // Act
      statisticsManager.WriteToLog(mockLogManager);

      // Assert
      mockLogManager.Received(1).WriteMessageToJobLog(Arg.Any<String>());
      mockLogManager.Received().WriteMessageToJobLog("0 Record(s) processed.");
    }
    #endregion
  }
}
