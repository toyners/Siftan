
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class OutputBuilder : IBuilder
  {
    public const String OutputKey = "output";

    public const String MatchedOutputKey = "-fm";

    public const String UnmatchedOutputKey = "-fu";

    private List<String> tokens = new List<String> { OutputKey };

    public String[] Build()
    {
      return this.tokens.ToArray();
    }

    public OutputBuilder HasMatchedOutputFile(String filePath)
    {
      this.tokens.Add(MatchedOutputKey);
      this.tokens.Add(filePath);
      return this;
    }

    public OutputBuilder HasUnmatchedOutputFile(String filePath)
    {
      this.tokens.Add(UnmatchedOutputKey);
      this.tokens.Add(filePath);
      return this;
    }
  }
}
