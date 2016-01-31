
namespace Siftan.Console
{
  using System;
  using System.Collections;

  public class Options
  {
    public const String UnrecognisedNounMessageTemplate = "'{0}' is not a recognised noun in command line arguments.";

    #region Construction
    public Options(String[] args)
    {
      if (args.Length == 0)
      {
        throw new Exception("No command line arguments.");
      }

      Queue queue = new Queue(args);

      this.Input = new InputOptions(queue);

      while (queue.Count > 0)
      {
        String noun = queue.Dequeue() as String;

        switch (noun)
        {
          case "delim":
          {
            this.Delimited = new DelimitedOptions(queue);
            break;
          }

          case "fixed":
          {
            this.FixedWidth = new FixedWidthOptions(queue);
            break;
          }

          case "inlist":
          {
            this.InList = new InListOptions(queue);
            break;
          }

          case "output":
          {
            this.Output = new OutputOptions(queue);
            break;
          }

          case "log":
          {
            this.Log = new LogOptions(queue);
            break;
          }

          default:
          {
            throw new Exception(String.Format(UnrecognisedNounMessageTemplate, noun));
          }
        }
      }

      if (this.Delimited != null && this.FixedWidth != null)
      {
        throw new Exception("Cannot have both 'delim' and 'fixed' record descriptor terms.");
      }

      if (this.Delimited == null && this.FixedWidth == null)
      {
        throw new Exception("Missing required record descriptor term. Use either 'delim' or 'fixed'.");
      }

      if (this.InList == null)
      {
        throw new Exception("Missing required match descriptor term. Use 'inlist'.");
      }

      if (this.Output == null)
      {
        throw new Exception("Missing required output descriptor term. Use 'output'.");
      }
    }
    #endregion

    #region Properties
    public InputOptions Input { get; private set; }

    public DelimitedOptions Delimited { get; private set; }

    public FixedWidthOptions FixedWidth { get; private set; }

    public InListOptions InList { get; private set; }

    public OutputOptions Output { get; private set; }

    public LogOptions Log { get; private set; }

    public Boolean HasApplicationLogFilePath { get { return this.Log != null && this.Log.ApplicationLogFilePath != null; } }

    public Boolean HasJobLogFilePath { get { return this.Log != null && this.Log.JobLogFilePath != null; } }

    public Boolean HasMatchedOutput { get { return this.Output != null && this.Output.FileMatched != null; } }
    #endregion

    #region Classes
    public class InputOptions
    {
      internal InputOptions(Queue queue)
      {
        this.Pattern = QueueOperations.DequeueArgument(queue);
        String field = queue.Peek() as String;
        if (field == "-r")
        {
          queue.Dequeue();
          this.SearchSubdirectories = true;
        }
      }

      public String Pattern { get; private set; }

      public Boolean SearchSubdirectories { get; private set; }
    }

    public class DelimitedOptions
    {
      #region Construction
      internal DelimitedOptions(Queue queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek() as String;
          switch (field)
          {
            case "-d":
            {
              QueueOperations.DequeueArgument(queue);
              this.Delimiter = QueueOperations.DequeueArgument(queue, "-d");
              break;
            }

            case "-q":
            {
              QueueOperations.DequeueArgument(queue);
              this.Qualifier = QueueOperations.DequeueChar(queue, "-q");
              break;
            }

            case "-h":
            {
              QueueOperations.DequeueArgument(queue);
              this.HeaderLineID = QueueOperations.DequeueArgument(queue, "-h");
              break;
            }

            case "-ti":
            {
              QueueOperations.DequeueArgument(queue);
              this.TermIndex = QueueOperations.DequeueUInt32(queue, "-ti");
              break;
            }

            case "-li":
            {
              QueueOperations.DequeueArgument(queue);
              this.LineIDIndex = QueueOperations.DequeueUInt32(queue, "-li");
              break;
            }

            case "-t":
            {
              QueueOperations.DequeueArgument(queue);
              this.TermLineID = QueueOperations.DequeueArgument(queue, "-t");
              break;
            }

            default:
            {
              parsingComplete = true;
              break;
            }
          }
        }

        if (this.Delimiter == null)
        {
          this.Delimiter = ",";
        }

        if (this.HeaderLineID == null)
        {
          throw new Exception("Missing required term 'Header ID' (-h).");
        }

        if (this.TermLineID == null)
        {
          throw new Exception("Missing required term 'Term Line ID' (-t).");
        }
      }
      #endregion

      #region Properties
      public String Delimiter { get; private set; }

      public Char Qualifier { get; private set; }

      public String HeaderLineID { get; private set; }

      public UInt32 LineIDIndex { get; private set; }

      public String TermLineID { get; private set; }

      public UInt32 TermIndex { get; private set; }
      #endregion
    }

    public class FixedWidthOptions
    {
      #region Construction
      internal FixedWidthOptions(Queue queue)
      {
        Boolean parsingComplete = false;
        Boolean gotLineStart = false;
        Boolean gotLineLength = false;
        Boolean gotTermStart = false;
        Boolean gotTermLength = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek() as String;
          switch (field)
          {
            case "-h":
            {
              QueueOperations.DequeueArgument(queue);
              this.HeaderLineID = QueueOperations.DequeueArgument(queue, "-h");
              break;
            }

            case "-ls":
            {
              QueueOperations.DequeueArgument(queue);
              this.LineIDStart = QueueOperations.DequeueUInt32(queue, "-hs");
              gotLineStart = true;
              break;
            }

            case "-ll":
            {
              QueueOperations.DequeueArgument(queue);
              this.LineIDLength = QueueOperations.DequeueUInt32(queue, "-hl");
              gotLineLength = true;
              break;
            }

            case "-t":
            {
              QueueOperations.DequeueArgument(queue);
              this.TermLineID = QueueOperations.DequeueArgument(queue, "-t");
              break;
            }

            case "-ts":
            {
              QueueOperations.DequeueArgument(queue);
              this.TermStart = QueueOperations.DequeueUInt32(queue, "-ts");
              gotTermStart = true;
              break;
            }

            case "-tl":
            {
              QueueOperations.DequeueArgument(queue);
              this.TermLength = QueueOperations.DequeueUInt32(queue, "-tl");
              gotTermLength = true;
              break;
            }

            default:
            {
              parsingComplete = true;
              break;
            }
          }
        }

        if (this.HeaderLineID == null)
        {
          throw new Exception("Missing required term 'Header ID' (-h).");
        }

        if (this.TermLineID == null)
        {
          throw new Exception("Missing required term 'Term Line ID' (-t).");
        }

        if (!gotLineStart)
        {
          throw new Exception("Missing required term 'Line ID Start' (-ls).");
        }

        if (!gotLineLength)
        {
          throw new Exception("Missing required term 'Line ID Length' (-ll).");
        }

        if (!gotTermStart)
        {
          throw new Exception("Missing required term 'Term Start' (-ts).");
        }

        if (!gotTermLength)
        {
          throw new Exception("Missing required term 'Term Length' (-tl).");
        }
      }
      #endregion

      #region Properties
      public String HeaderLineID { get; private set; }

      public UInt32 LineIDStart { get; private set; }

      public UInt32 LineIDLength { get; private set; }

      public String TermLineID { get; private set; }

      public UInt32 TermStart { get; private set; }

      public UInt32 TermLength { get; private set; }
      #endregion
    }

    public class InListOptions
    {
      #region Construction
      internal InListOptions(Queue queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek() as String;
          switch (field)
          {
            case "-f":
            {
              QueueOperations.DequeueArgument(queue);
              this.FilePath = QueueOperations.DequeueArgument(queue, "-f");
              break;
            }

            case "-m":
            {
              QueueOperations.DequeueArgument(queue);
              this.MatchQuota = QueueOperations.DequeueEnum<InListExpression.MatchQuotas>(queue, "-m");
              break;
            }

            case "-v":
            {
              QueueOperations.DequeueArgument(queue);
              this.Values = QueueOperations.DequeueArray(queue, ':', "-v");
              break;
            }

            default:
            {
              parsingComplete = true;
              break;
            }
          }
        }

        if (this.FilePath != null && this.Values != null)
        {
          throw new Exception("Cannot have both 'File Path' (-f) and 'Value List' (-v) terms.");
        }

        if (this.FilePath == null && this.Values == null)
        {
          throw new Exception("Missing one required term. Must have 'File Path' (-f) or 'Value List' (-v) but not both.");
        }
      }
      #endregion

      #region Properties
      public String[] Values { get; private set; }

      public InListExpression.MatchQuotas MatchQuota { get; private set; }

      public String FilePath { get; private set; }

      public Boolean UseFile { get { return this.FilePath != null; } }
      #endregion
    }

    public class OutputOptions
    {
      #region Construction
      internal OutputOptions(Queue queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek() as String;
          switch (field)
          {
            case "-fm":
            {
              QueueOperations.DequeueArgument(queue);
              this.FileMatched = QueueOperations.DequeueArgument(queue, "-fm");
              break;
            }

            case "-fu":
            {
              QueueOperations.DequeueArgument(queue);
              this.FileUnmatched = QueueOperations.DequeueArgument(queue, "-fu");
              break;
            }

            default:
            {
              parsingComplete = true;
              break;
            }
          }
        }

        if (this.FileMatched == null && this.FileUnmatched == null)
        {
          throw new Exception("Missing required file term. Use either '-fm' or '-fu' or both.");
        }
      }
      #endregion

      #region Properties
      public String FileMatched { get; private set; }

      public String FileUnmatched { get; private set; }
      #endregion
    }

    public class LogOptions
    {
      internal LogOptions(Queue queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek() as String;
          switch (field)
          {
            case "-a":
            {
              QueueOperations.DequeueArgument(queue);
              this.ApplicationLogFilePath = QueueOperations.DequeueArgument(queue, "-a");
              break;
            }

            case "-j":
            {
              QueueOperations.DequeueArgument(queue);
              this.JobLogFilePath = QueueOperations.DequeueArgument(queue, "-j");
              break;
            }

            default:
            {
              parsingComplete = true;
              break;
            }
          }
        }
      }

      public String ApplicationLogFilePath { get; private set; }

      public String JobLogFilePath { get; internal set; }
    }
    #endregion
  }
}
