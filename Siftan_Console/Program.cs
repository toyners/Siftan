
namespace Siftan_Console
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;
  using Siftan;

  public class Program
  {
    public static void Main(String[] args)
    {
      Options options = new Options(args);

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

      OneFileRecordWriter recordWriter = new OneFileRecordWriter(options.Output.FileMatched, options.Output.FileUnmatched);

      String logFilePath = inputFilePaths[0] + ".log";
        
      new Engine().Execute(
        inputFilePaths, 
        new LogManager(options.Log.ApplicationLogFilePath, options.Log.JobLogFilePath), 
        new FileReaderFactory(), 
        recordReader, 
        expression, 
        recordWriter);

      recordWriter.Close();
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
  }
}
