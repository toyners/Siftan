
namespace Siftan.TestSupport
{
  using System;

  public static class CommandLineArgumentsCreator
  {
    public static String CreateForDelimitedTests(
      String inputFilePath,
      String headerLineID,
      String termLineID,
      String value,
      String matchedOutputFilePath,
      String unmatchedOutputFilePath,
      LogBuilder logBuilder)
    {
      var commandLineArgumentsBuilder = new CommandLineArgumentsBuilder()
        .WithInput(new InputBuilder()
          .IsSingleFile(inputFilePath))
        .WithDelim(new DelimBuilder()
          .HasHeaderLineID(headerLineID)
          .HasTermLineID(termLineID))
        .WithInList(new InListBuilder()
          .HasValuesList(value))
        .WithOutput(new OutputBuilder()
          .HasMatchedOutputFile(matchedOutputFilePath)
          .HasUnmatchedOutputFile(unmatchedOutputFilePath));

      if (logBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithLog(logBuilder);
      }

      return String.Join(" ", commandLineArgumentsBuilder.Build());
    }

    public static LogBuilder CreateLogBuilder(String applicationLogFilePath, String jobLogFilePath)
    {
      LogBuilder logBuilder = null;
      if (applicationLogFilePath != null || jobLogFilePath != null)
      {
        logBuilder = new LogBuilder();

        if (applicationLogFilePath != null)
        {
          logBuilder = logBuilder.HasApplicationLogFilePath(applicationLogFilePath);
        }

        if (jobLogFilePath != null)
        {
          logBuilder = logBuilder.HasJobLogFilePath(jobLogFilePath);
        }
      }

      return logBuilder;
    }
  }
}
