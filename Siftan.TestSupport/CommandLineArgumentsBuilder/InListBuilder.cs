
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class InListBuilder : IBuilder
  {
    public const String InListKey = "inlist";

    public const String FileKey = "-f";

    public const String ValuesKey = "-v";

    public const String QuotaKey = "-m";

    private readonly List<String> tokens = new List<String> { InListKey };

    public String[] Build()
    {
      return this.tokens.ToArray();
    }

    public InListBuilder HasValuesFile(String filePath)
    {
      this.tokens.Add(FileKey);
      this.tokens.Add(filePath);
      return this;
    }

    public InListBuilder HasValuesList(String list)
    {
      this.tokens.Add(ValuesKey);
      this.tokens.Add(list);
      return this;
    }

    public InListBuilder HasMatchQuota(InListExpression.MatchQuotas quota = InListExpression.MatchQuotas.None)
    {
      this.tokens.Add(QuotaKey);
      this.tokens.Add(quota.ToString());
      return this;
    }
  }
}
