
namespace Siftan.IntegrationTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class LogManager_IntegrationTests
  {
    private const String FirstLogMessage = "Test message #1";

    private const String SecondLogMessage = "Test message #2";

    private const String ThirdLogMessage = "Test message #3";

    private String parentDirectory;

    private String applicationLogFilePath;

    private String jobLogFilePath;

    #region Methods
    [SetUp]
    public void SetupBeforeEachTest()
    {
      this.parentDirectory = Path.GetTempPath() + Path.GetRandomFileName();
      Directory.CreateDirectory(this.parentDirectory);

      this.applicationLogFilePath = this.parentDirectory + @"\ApplicationLogFile.log";

      this.jobLogFilePath = this.parentDirectory + @"\JobLogFile.log";
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
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      // Act
      logManager.WriteMessage(LogEntryTypes.Application, FirstLogMessage);
      logManager.WriteMessage(LogEntryTypes.Application, SecondLogMessage);
      logManager.WriteMessage(LogEntryTypes.Application, ThirdLogMessage);
      logManager.Close();

      // Assert
      File.Exists(this.applicationLogFilePath).Should().BeTrue();

      String[] logFileLines = null;
      Action action = () => logFileLines = File.ReadAllLines(this.applicationLogFilePath);
      action.ShouldNotThrow();

      logFileLines.Length.Should().Be(3);
      logFileLines[0].Should().Be(FirstLogMessage);
      logFileLines[1].Should().Be(SecondLogMessage);
      logFileLines[2].Should().Be(ThirdLogMessage);
    }

    [Test]
    public void MessagesInOpenApplicationLogCanBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        logManager.WriteMessage(LogEntryTypes.Application, FirstLogMessage, LogEntryFlushTypes.Force);
        logManager.WriteMessage(LogEntryTypes.Application, SecondLogMessage, LogEntryFlushTypes.Force);
        logManager.WriteMessage(LogEntryTypes.Application, ThirdLogMessage, LogEntryFlushTypes.Force);

        // Assert
        File.Exists(this.applicationLogFilePath).Should().BeTrue();

        List<String> logFileLines = null;
        Action action = () => logFileLines = ReadAllLinesFromOpenLogFile(this.applicationLogFilePath);
        action.ShouldNotThrow();

        logFileLines.Count.Should().Be(3);
        logFileLines[0].Should().Be(FirstLogMessage);
        logFileLines[1].Should().Be(SecondLogMessage);
        logFileLines[2].Should().Be(ThirdLogMessage);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    [Test]
    public void MessagesInOpenApplicationLogNotFlushedCannotBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        logManager.WriteMessage(LogEntryTypes.Application, FirstLogMessage, LogEntryFlushTypes.Lazy);
        logManager.WriteMessage(LogEntryTypes.Application, SecondLogMessage, LogEntryFlushTypes.Lazy);
        logManager.WriteMessage(LogEntryTypes.Application, ThirdLogMessage, LogEntryFlushTypes.Lazy);

        // Assert
        File.Exists(this.applicationLogFilePath).Should().BeTrue();

        List<String> logFileLines = null;
        Action action = () => logFileLines = ReadAllLinesFromOpenLogFile(this.applicationLogFilePath);
        action.ShouldNotThrow();

        logFileLines.Count.Should().Be(0);
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
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      // Act
      logManager.WriteMessage(LogEntryTypes.Job, FirstLogMessage);
      logManager.WriteMessage(LogEntryTypes.Job, SecondLogMessage);
      logManager.WriteMessage(LogEntryTypes.Job, ThirdLogMessage);
      logManager.Close();

      // Assert
      File.Exists(this.jobLogFilePath).Should().BeTrue();
      String[] logLines = File.ReadAllLines(this.jobLogFilePath);

      logLines.Length.Should().Be(3);
      logLines[0].Should().Be(FirstLogMessage);
      logLines[1].Should().Be(SecondLogMessage);
      logLines[2].Should().Be(ThirdLogMessage);
    }

    [Test]
    public void MessagesInOpenJobLogCanBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        logManager.WriteMessage(LogEntryTypes.Job, FirstLogMessage, LogEntryFlushTypes.Force);
        logManager.WriteMessage(LogEntryTypes.Job, SecondLogMessage, LogEntryFlushTypes.Force);
        logManager.WriteMessage(LogEntryTypes.Job, ThirdLogMessage, LogEntryFlushTypes.Force);

        // Assert
        File.Exists(this.jobLogFilePath).Should().BeTrue();

        List<String> logFileLines = null;
        Action action = () => logFileLines = ReadAllLinesFromOpenLogFile(this.jobLogFilePath);
        action.ShouldNotThrow();

        logFileLines.Count.Should().Be(3);
        logFileLines[0].Should().Be(FirstLogMessage);
        logFileLines[1].Should().Be(SecondLogMessage);
        logFileLines[2].Should().Be(ThirdLogMessage);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    [Test]
    public void MessagesInOpenJobLogNotFlushedCannotBeReadByOtherReader()
    {
      // Arrange
      LogManager logManager = new LogManager(this.applicationLogFilePath, this.jobLogFilePath);

      try
      {
        // Act
        logManager.WriteMessage(LogEntryTypes.Job, FirstLogMessage, LogEntryFlushTypes.Lazy);
        logManager.WriteMessage(LogEntryTypes.Job, SecondLogMessage, LogEntryFlushTypes.Lazy);
        logManager.WriteMessage(LogEntryTypes.Job, ThirdLogMessage, LogEntryFlushTypes.Lazy);

        // Assert
        File.Exists(this.jobLogFilePath).Should().BeTrue();

        List<String> logFileLines = null;
        Action action = () => logFileLines = ReadAllLinesFromOpenLogFile(this.jobLogFilePath);
        action.ShouldNotThrow();

        logFileLines.Count.Should().Be(0);
      }
      finally
      {
        // Teardown
        logManager.Close();
      }
    }

    private static List<String> ReadAllLinesFromOpenLogFile(String filePath)
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

      return lines;
    }
    #endregion
  }
}
