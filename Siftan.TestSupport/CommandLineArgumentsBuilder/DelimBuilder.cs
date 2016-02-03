
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class DelimBuilder : IBuilder
  {
    public const String DelimKey = "delim";

    public const String DelimiterKey = "-d";

    public const String QualifierKey = "-q";

    public const String HeaderLineIDKey = "-h";

    public const String LineIDIndexKey = "-li";

    public const String TermLineIDKey = "-t";

    public const String TermIndexKey = "-ti";

    public const String DefaultDelimiter = ",";

    private List<String> tokens = new List<String> { DelimKey };

    public String[] Build()
    {
      return this.tokens.ToArray();
    }

    public DelimBuilder HasDelimiter(String delimiter = null)
    {
      this.tokens.Add(DelimiterKey);

      if (delimiter == null)
      {
        this.tokens.Add(DefaultDelimiter);
      }
      else
      {
        this.tokens.Add(delimiter);
      }

      return this;
    }

    public DelimBuilder HasQualifier(Char qualifier)
    {
      this.tokens.Add(QualifierKey);
      this.tokens.Add(qualifier.ToString());
      return this;
    }

    public DelimBuilder HasHeaderLineID(String headerLineID)
    {
      this.tokens.Add(HeaderLineIDKey);
      this.tokens.Add(headerLineID);
      return this;
    }

    public DelimBuilder HasLineIDIndex(UInt32 lineIndexID)
    {
      this.tokens.Add(LineIDIndexKey);
      this.tokens.Add(lineIndexID.ToString());
      return this;
    }

    public DelimBuilder HasTermLineID(String termLineID)
    {
      this.tokens.Add(TermLineIDKey);
      this.tokens.Add(termLineID);
      return this;
    }

    public DelimBuilder HasTermIndex(UInt32 termIndex)
    {
      this.tokens.Add(TermIndexKey);
      this.tokens.Add(termIndex.ToString());
      return this;
    }
  }
}
