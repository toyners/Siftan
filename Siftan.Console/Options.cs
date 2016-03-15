
namespace Siftan.Console
{
  using System;
  using System.Collections.Generic;

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

      Queue<String> queue = new Queue<String>(args);

      this.Input = new InputOptions(queue);

      while (queue.Count > 0)
      {
        String noun = queue.Dequeue();

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
      internal InputOptions(Queue<String> queue)
      {
        this.Pattern = queue.Dequeue();
        String field = queue.Peek();
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
      internal DelimitedOptions(Queue<String> queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek();
          switch (field)
          {
            case "-d":
            {
              queue.Dequeue();
              this.Delimiter = queue.DequeueArgument("-d");
              break;
            }

            case "-q":
            {
              queue.Dequeue();
              this.Qualifier = queue.DequeueChar("-q");
              break;
            }

            case "-h":
            {
              queue.Dequeue();
              this.HeaderLineID = queue.DequeueArgument("-h");
              break;
            }

            case "-ti":
            {
              queue.Dequeue();
              this.TermIndex = queue.DequeueUInt32("-ti");
              break;
            }

            case "-li":
            {
              queue.Dequeue();
              this.LineIDIndex = queue.DequeueUInt32("-li");
              break;
            }

            case "-t":
            {
              queue.Dequeue();
              this.TermLineID = queue.DequeueArgument("-t");
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
      internal FixedWidthOptions(Queue<String> queue)
      {
        Boolean parsingComplete = false;
        Boolean gotLineStart = false;
        Boolean gotLineLength = false;
        Boolean gotTermStart = false;
        Boolean gotTermLength = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek();
          switch (field)
          {
            case "-h":
            {
              queue.Dequeue();
              this.HeaderLineID = queue.DequeueArgument("-h");
              break;
            }

            case "-ls":
            {
              queue.Dequeue();
              this.LineIDStart = queue.DequeueUInt32("-hs");
              gotLineStart = true;
              break;
            }

            case "-ll":
            {
              queue.Dequeue();
              this.LineIDLength = queue.DequeueUInt32("-hl");
              gotLineLength = true;
              break;
            }

            case "-t":
            {
              queue.Dequeue();
              this.TermLineID = queue.DequeueArgument("-t");
              break;
            }

            case "-ts":
            {
              queue.Dequeue();
              this.TermStart = queue.DequeueUInt32("-ts");
              gotTermStart = true;
              break;
            }

            case "-tl":
            {
              queue.Dequeue();
              this.TermLength = queue.DequeueUInt32("-tl");
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
      internal InListOptions(Queue<String> queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek();
          switch (field)
          {
            case "-f":
            {
              queue.Dequeue();
              this.FilePath = queue.DequeueArgument("-f");
              break;
            }

            case "-m":
            {
              queue.Dequeue();
              this.MatchQuota = queue.DequeueEnum<InListExpression.MatchQuotas>("-m");
              break;
            }

            case "-v":
            {
              queue.Dequeue();
              this.Values = queue.DequeueArray(':', "-v");
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
      internal OutputOptions(Queue<String> queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek();
          switch (field)
          {
            case "-fm":
            {
              queue.Dequeue();
              this.FileMatched = queue.DequeueArgument("-fm");
              break;
            }

            case "-fu":
            {
              queue.Dequeue();
              this.FileUnmatched = queue.DequeueArgument("-fu");
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
      internal LogOptions(Queue<String> queue)
      {
        Boolean parsingComplete = false;
        while (queue.Count > 0 && !parsingComplete)
        {
          String field = queue.Peek();
          switch (field)
          {
            case "-a":
            {
              queue.Dequeue();
              this.ApplicationLogFilePath = queue.DequeueArgument("-a");
              break;
            }

            case "-j":
            {
              queue.Dequeue();
              this.JobLogFilePath = queue.DequeueArgument("-j");
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
