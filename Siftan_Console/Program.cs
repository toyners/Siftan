﻿
namespace Siftan_Console
{
  using System;
  using System.IO;
  using System.Reflection;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Path;
  using Siftan;

  public class Program
  {
    public static void Main(String[] args)
    {
      LogManager logManager = new LogManager(CreateDefaultApplicationLogFilePath());

      try
      {
        Options options = new Options(args);

        CompleteLogConfiguring(options, logManager);

        String[] inputFilePaths = GetInputFilePaths(options);

        IRecordReader recordReader = null;
        if (options.Delimited != null)
        {
          DelimitedRecordDescriptor recordDescriptor = CreateDelimitedRecordDescriptor(options.Delimited);
          recordReader = new DelimitedRecordReader(recordDescriptor);
        }
        else
        {
          // Set up fixed width record reader.
        }

        IRecordMatchExpression expression = null;
        if (options.InList.UseFile)
        {
          // Load file contents into in list expression.
        }
        else
        {
          expression = new InListExpression(options.InList.Values);
        }

        StatisticsManager statisticsManager = new StatisticsManager();

        OneFileRecordWriter recordWriter = new OneFileRecordWriter(
          options.Output.FileMatched,
          options.Output.FileUnmatched,
          statisticsManager);

        new Engine().Execute(
          inputFilePaths,
          logManager,
          new FileReaderFactory(),
          recordReader,
          expression,
          recordWriter,
          statisticsManager,
          statisticsManager);

        recordWriter.Close();
      }
      catch (Exception exception)
      {
        logManager.WriteMessageToApplicationLog("EXCEPTION: " + exception.Message);
        logManager.WriteMessageToApplicationLog("STACK: " + exception.StackTrace);
        throw;
      }
      finally
      {
        logManager.Close();
      }
    }

    private static void CompleteLogConfiguring(Options options, LogManager logManager)
    {
      if (options.HasApplicationLogFilePath)
      {
        logManager.ApplicationLogFilePath = options.Log.ApplicationLogFilePath;
      }

      if (options.HasJobLogFilePath)
      {
        logManager.JobLogFilePath = options.Log.JobLogFilePath;
      }
      else if (options.HasMatchedOutput)
      {
        logManager.JobLogFilePath =
          PathOperations.CompleteDirectoryPath(
          Path.GetDirectoryName(options.Output.FileMatched)) +
          "Job.log";
      }
      else
      {
        logManager.JobLogFilePath =
          PathOperations.CompleteDirectoryPath(
          Path.GetDirectoryName(options.Output.FileUnmatched)) +
          "Job.log";
      }
    }

    private static String[] GetInputFilePaths(Options options)
    {
      FilePatternResolver.SearchDepths searchDepth = options.Input.SearchSubdirectories ? FilePatternResolver.SearchDepths.AllDirectories : FilePatternResolver.SearchDepths.InitialDirectoryOnly;
      String[] inputFilePaths = new FilePatternResolver().ResolveFilePattern(options.Input.Pattern, searchDepth);

      if (inputFilePaths.Length == 0)
      {
        throw new FileNotFoundException(String.Format("No files found matching pattern '{0}'.", options.Input.Pattern));
      }

      return inputFilePaths;
    }

    private static DelimitedRecordDescriptor CreateDelimitedRecordDescriptor(Options.DelimitedOptions delimitedOptions)
    {
      return new DelimitedRecordDescriptor
      {
        Delimiter = delimitedOptions.Delimiter,
        Qualifier = delimitedOptions.Qualifier,
        HeaderID = delimitedOptions.HeaderLineID,
        LineIDIndex = delimitedOptions.LineIDIndex,
        DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition(delimitedOptions.TermLineID, delimitedOptions.TermIndex)
      };
    }

    public static String CreateDefaultApplicationLogFilePath()
    {
      String assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return PathOperations.CompleteDirectoryPath(assemblyDirectory) +
             DateTime.Today.ToString("dd-MM-yyyy") + ".log";
    }
  }
}
