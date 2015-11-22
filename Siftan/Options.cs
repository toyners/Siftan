
namespace Siftan
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class Options
  {
    #region Construction
    public Options(String[] args)
    {
      Queue queue = new Queue(args);

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

        default:
        {
          throw new Exception("No recognised noun found in command line arguments.");
        }
      }
    }
    #endregion

    #region Propertie
    public DelimitedOptions Delimited { get; private set; }

    public FixedWidthOptions FixedWidth { get; private set; }

    public InListOptions InList { get; private set; }

    public String[] Files { get; private set; }
    #endregion

    #region Classes
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
      #endregion
    }
    #endregion
  }
}
