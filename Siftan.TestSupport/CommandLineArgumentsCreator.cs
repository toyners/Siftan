
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
      var args = CreateArgumentsForDelimitedTests(
        inputFilePath,
        headerLineID,
        termLineID,
        value,
        matchedOutputFilePath,
        unmatchedOutputFilePath,
        logBuilder);

      return String.Join(" ", args);
    }

    public static String[] CreateArgumentsForDelimitedTests(
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

      return commandLineArgumentsBuilder.Build();
    }

    public static String[] CreateArgumentsForDelimitedTests(
      InputBuilder inputBuilder,
      String headerLineID,
      String termLineID,
      String value,
      OutputBuilder outputBuilder,
      LogBuilder logBuilder)
    {
      var commandLineArgumentsBuilder = new CommandLineArgumentsBuilder();

      if (inputBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithInput(inputBuilder);
      }

      commandLineArgumentsBuilder.WithDelim(new DelimBuilder()
                                              .HasHeaderLineID(headerLineID)
                                              .HasTermLineID(termLineID));

      commandLineArgumentsBuilder.WithInList(new InListBuilder()
                                              .HasValuesList(value));

      if (outputBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithOutput(outputBuilder);
      }

      if (logBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithLog(logBuilder);
      }

      return commandLineArgumentsBuilder.Build();
    }

    public static String[] CreateArgumentsForDelimitedTests(
      InputBuilder inputBuilder,
      DelimBuilder delimBuilder,
      String value,
      OutputBuilder outputBuilder,
      LogBuilder logBuilder)
    {
      var commandLineArgumentsBuilder = new CommandLineArgumentsBuilder();

      if (inputBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithInput(inputBuilder);
      }

      if (delimBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithDelim(delimBuilder);
      }

      commandLineArgumentsBuilder.WithInList(new InListBuilder()
                                              .HasValuesList(value));

      if (outputBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithOutput(outputBuilder);
      }

      if (logBuilder != null)
      {
        commandLineArgumentsBuilder = commandLineArgumentsBuilder.WithLog(logBuilder);
      }

      return commandLineArgumentsBuilder.Build();
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

    public static InputBuilder CreateSingleFileInputBuilder(String singleFilePath, Boolean searchSubDirectories = false)
    {
      InputBuilder inputBuilder = new InputBuilder();
      inputBuilder = inputBuilder.IsSingleFile(singleFilePath);

      return MayAddSearchSubDirectories(inputBuilder, searchSubDirectories);
    }

    public static InputBuilder CreateMultipleFilesInputBuilder(String multipleFilesPattern, Boolean searchSubDirectories = false)
    {
      InputBuilder inputBuilder = new InputBuilder();
      inputBuilder = inputBuilder.IsMultipleFiles(multipleFilesPattern);

      return MayAddSearchSubDirectories(inputBuilder, searchSubDirectories);
    }

    public static OutputBuilder CreateOutputBuilder(String matchedOutputFilePath, String unmatchedOutputFilePath)
    {
      OutputBuilder outputBuilder = new OutputBuilder();

      if (matchedOutputFilePath != null)
      {
        outputBuilder = outputBuilder.HasMatchedOutputFile(matchedOutputFilePath);
      }

      if (unmatchedOutputFilePath != null)
      {
        outputBuilder = outputBuilder.HasUnmatchedOutputFile(unmatchedOutputFilePath);
      }

      return outputBuilder;
    }

    public static DelimBuilder CreateDelimBuilder(String delimiter, Char qualifier, String headerLineID, UInt32 lineIDIndex, String termLineID, UInt32 termIndex)
    {
      return new DelimBuilder().HasDelimiter(delimiter)
                               .HasQualifier(qualifier)
                               .HasHeaderLineID(headerLineID)
                               .HasLineIDIndex(lineIDIndex)
                               .HasTermLineID(termLineID)
                               .HasTermIndex(termIndex);
    }

    private static InputBuilder MayAddSearchSubDirectories(InputBuilder inputBuilder, Boolean searchSubDirectories)
    {
      if (searchSubDirectories)
      {
        inputBuilder = inputBuilder.AndSearchSubDirectories();
      }

      return inputBuilder;
    }
  }
}