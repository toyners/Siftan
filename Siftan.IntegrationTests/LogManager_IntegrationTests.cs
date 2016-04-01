
namespace Siftan.IntegrationTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Text.RegularExpressions;
  using FluentAssertions;
  using Jabberwocky.Toolkit.IO;
  using NSubstitute;
  using NUnit.Framework;
  using Shouldly;
  using TestSupport;

  [TestFixture]
  public class LogManager_IntegrationTests
  {
    private const String FirstLogMessage = "Test message #1";

    private const String SecondLogMessage = "Test message #2";

    private const String ThirdLogMessage = "Test message #3";

    private const String LateNewYearsEveDateTimeStamp = "[31-12-2015 23:59:59]";

    private const String EarlyNewYearsDayDateTimeStamp = "[01-01-2016 00:00:01]";

    private const String NewYearsDayDateTimeStamp = "[01-01-2016 11:23:45]";

    private static String[] ExpectedLogFileContents;

    private String workingDirectory;

    private String applicationLogFilePath;

    private String alternativeApplicationLogFilePath;

    private String jobLogFilePath;

    private String alternativeJobLogFilePath;

    private IDateTimeStamper mockDateTimeStamper;

    #region Methods
    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"Siftan.IntegrationTests\";
      this.applicationLogFilePath = this.workingDirectory + @"\ApplicationLogFile.log";
      this.alternativeApplicationLogFilePath = this.workingDirectory + @"\AlternativeApplicationLogFile.log";
      this.jobLogFilePath = this.workingDirectory + @"\JobLogFile.log";
      this.alternativeJobLogFilePath = this.workingDirectory + @"\AlternativeJobLogFile.log";

      ExpectedLogFileContents = new String[]
      {
        Regex.Escape(LateNewYearsEveDateTimeStamp) + " " + FirstLogMessage,
        Regex.Escape(EarlyNewYearsDayDateTimeStamp) + " " + SecondLogMessage,
        Regex.Escape(NewYearsDayDateTimeStamp) + " " + ThirdLogMessage,
      };
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      DirectoryOperations.EnsureDirectoryIsEmpty(this.workingDirectory);

      this.mockDateTimeStamper = Substitute.For<IDateTimeStamper>();
      this.mockDateTimeStamper.Now.Returns(LateNewYearsEveDateTimeStamp, EarlyNewYearsDayDateTimeStamp, NewYearsDayDateTimeStamp);
    }

    [Test]
    public void WritingMessagesToApplicationLogAndClosingCreatesValidLog()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);

      // Act
      WriteMessagesToApplicationLog(logManager);
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
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);

      try
      {
        // Act
        WriteMessagesToApplicationLog(logManager);

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
    public void ApplicationLogFileNotCreatedWhenLogManagerIsInstantiated()
    {
      // Act
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.Close();

      // Assert
      File.Exists(this.applicationLogFilePath).Should().BeFalse();
    }

    [Test]
    public void ChangingApplicationLogFilePathResultsInANewLogBeingCreated()
    {
      // Act
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.WriteMessageToApplicationLog("Application log file.");
      logManager.Close();

      logManager.ApplicationLogFilePath = this.alternativeApplicationLogFilePath;
      logManager.WriteMessageToApplicationLog("Alteranative application log file.");
      logManager.Close();

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.alternativeApplicationLogFilePath).ShouldBeTrue();
    }

    [Test]
    public void WritingMessagesToJobLogAndClosingCreatesValidLog()
    {
      // Arrange
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.JobLogFilePath = this.jobLogFilePath;

      // Act
      WriteMessagesToJobLog(logManager);
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
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.JobLogFilePath = this.jobLogFilePath;

      try
      {
        // Act
        WriteMessagesToJobLog(logManager);

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
    public void JobLogFileNotCreatedWhenLogManagerIsInstantiated()
    {
      // Act
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.JobLogFilePath = this.jobLogFilePath;
      logManager.Close();

      // Assert
      File.Exists(this.jobLogFilePath).Should().BeFalse();
    }

    [Test]
    public void ChangingJobLogFilePathResultsInANewLogBeingCreated()
    {
      // Act
      LogManager logManager = new LogManager(this.mockDateTimeStamper, this.applicationLogFilePath);
      logManager.JobLogFilePath = this.jobLogFilePath;
      logManager.WriteMessageToJobLog("Job log file.");
      logManager.Close();

      logManager.JobLogFilePath = this.alternativeJobLogFilePath;
      logManager.WriteMessageToJobLog("Alternative job log file.");
      logManager.Close();

      // Assert
      File.Exists(this.jobLogFilePath).ShouldBeTrue();
      File.Exists(this.alternativeJobLogFilePath).ShouldBeTrue();
    }

    private static void WriteMessagesToApplicationLog(LogManager logManager)
    {
      logManager.WriteMessageToApplicationLog(FirstLogMessage);
      logManager.WriteMessageToApplicationLog(SecondLogMessage);
      logManager.WriteMessageToApplicationLog(ThirdLogMessage);
    }

    private static void WriteMessagesToJobLog(LogManager logManager)
    {
      logManager.WriteMessageToJobLog(FirstLogMessage);
      logManager.WriteMessageToJobLog(SecondLogMessage);
      logManager.WriteMessageToJobLog(ThirdLogMessage);
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
      StringArrayComparison.IsMatching(logFileLines, ExpectedLogFileContents);
    }
    #endregion
  }
}
