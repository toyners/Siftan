
namespace Siftan.IntegrationTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using FluentAssertions;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class LogManager_IntegrationTests
  {
    private const String FirstLogMessage = "Test message #1";

    private const String SecondLogMessage = "Test message #2";

    private const String ThirdLogMessage = "Test message #3";

    private const String LateNewYearsEveDateTimeStamp = "[31-12-2015 23:59:59]";

    private const String EarlyNewYearsDayDateTimeStamp = "[01-01-2016 00:00:01]";

    private const String NewYearsDayDateTimeStamp = "[01-01-2016 11:23:45]";

    private String parentDirectory;

    private String applicationLogFilePath;

    private String jobLogFilePath;

    private IDateTimeStamper mockDateTimeStamper;

    #region Methods
    [SetUp]
    public void SetupBeforeEachTest()
    {
      this.parentDirectory = Path.GetTempPath() + Path.GetRandomFileName();
      Directory.CreateDirectory(this.parentDirectory);

      this.applicationLogFilePath = this.parentDirectory + @"\ApplicationLogFile.log";

      this.jobLogFilePath = this.parentDirectory + @"\JobLogFile.log";

      DateTime LateNewYearsEveDateTime = new DateTime(2015, 12, 31, 23, 59, 59);
      DateTime EarlyNewYearsDayDateTime = new DateTime(2016, 1, 1, 0, 0, 1);
      DateTime NewYearsDayDateTime = new DateTime(2016, 1, 1, 11, 23, 45);
      this.mockDateTimeStamper = Substitute.For<IDateTimeStamper>();
      this.mockDateTimeStamper.Now.Returns(LateNewYearsEveDateTime, EarlyNewYearsDayDateTime, NewYearsDayDateTime);
    }

    [TearDown]
    public void TeardownAfterEachTest()
    {
      if (Directory.Exists(this.parentDirectory))
      {
        Directory.Delete(this.parentDirectory, true);
      }
    }

    [Test]
    public void WritingMessagesToApplicationLogAndClosingCreatesValidLog()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      // Act
      WriteMessagesToLogManager(logManager, LogEntryTypes.Application, LogEntryFlushTypes.Lazy);
      logManager.Close();

      // Assert
      File.Exists(this.applicationLogFilePath).Should().BeTrue();

      String[] logFileLines = File.ReadAllLines(this.applicationLogFilePath);
      AssertLogFileContentsAreCorrect(logFileLines);
    }

    [Test]
    public void MessagesInOpenApplicationLogCanBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        WriteMessagesToLogManager(logManager, LogEntryTypes.Application, LogEntryFlushTypes.Force);

        // Assert
        File.Exists(this.applicationLogFilePath).Should().BeTrue();

        String[] logFileLines = GetOpenLogFileContent(this.applicationLogFilePath);
        AssertLogFileContentsAreCorrect(logFileLines);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    [Test]
    public void MessagesInOpenApplicationLogNotFlushedIsNotReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        WriteMessagesToLogManager(logManager, LogEntryTypes.Application, LogEntryFlushTypes.Lazy);

        // Assert
        File.Exists(this.applicationLogFilePath).Should().BeTrue();

        String[] logFileLines = GetOpenLogFileContent(this.applicationLogFilePath);
        logFileLines.Length.Should().Be(0);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    [Test]
    public void WritingMessagesToJobLogAndClosingCreatesValidLog()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      // Act
      WriteMessagesToLogManager(logManager, LogEntryTypes.Job, LogEntryFlushTypes.Lazy);
      logManager.Close();

      // Assert
      File.Exists(this.jobLogFilePath).Should().BeTrue();

      String[] logFileLines = File.ReadAllLines(this.jobLogFilePath);
      AssertLogFileContentsAreCorrect(logFileLines);
    }

    [Test]
    public void MessagesInOpenJobLogCanBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        WriteMessagesToLogManager(logManager, LogEntryTypes.Job, LogEntryFlushTypes.Force);

        // Assert
        File.Exists(this.jobLogFilePath).Should().BeTrue();

        String[] logFileLines = GetOpenLogFileContent(this.jobLogFilePath);
        AssertLogFileContentsAreCorrect(logFileLines);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    [Test]
    public void MessagesInOpenJobLogNotFlushedIsNotReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        WriteMessagesToLogManager(logManager, LogEntryTypes.Job, LogEntryFlushTypes.Lazy);

        // Assert
        File.Exists(this.jobLogFilePath).Should().BeTrue();

        String[] logFileLines = GetOpenLogFileContent(this.jobLogFilePath);
        logFileLines.Length.Should().Be(0);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    private static void WriteMessagesToLogManager(LogManager logManager, LogEntryTypes entryType, LogEntryFlushTypes flushType)
    {
      logManager.WriteMessage(entryType, FirstLogMessage, flushType);
      logManager.WriteMessage(entryType, SecondLogMessage, flushType);
      logManager.WriteMessage(entryType, ThirdLogMessage, flushType);
    }

    private static String[] GetOpenLogFileContent(String filePath)
    {
      String[] logFileLines = null;
      Action action = () =>
      {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write);

        List<String> lines = new List<String>();
        using (StreamReader sr = new StreamReader(fs))
        {
          while (!sr.EndOfStream)
          {
            lines.Add(sr.ReadLine());
          }
        }

        logFileLines = lines.ToArray();
      };
      
      // Explicitly checking that no exception is thrown is part of having informative diagnostics
      action.ShouldNotThrow();

      return logFileLines;
    }

    private static void AssertLogFileContentsAreCorrect(String[] logFileLines)
    {
      logFileLines.Length.Should().Be(3);
      logFileLines[0].Should().Be(LateNewYearsEveDateTimeStamp + " " + FirstLogMessage);
      logFileLines[1].Should().Be(EarlyNewYearsDayDateTimeStamp + " " + SecondLogMessage);
      logFileLines[2].Should().Be(NewYearsDayDateTimeStamp + " " + ThirdLogMessage);
    }
    #endregion
  }
}
