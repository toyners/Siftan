
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class LogBuilder : IBuilder
  {
    public const String LogKey = "log";

    public const String ApplicationKey = "-a";

    public const String JobKey = "-j";

    private List<String> tokens = new List<String> { LogKey };

    public String[] Build()
    {
      return this.tokens.ToArray();
    }

    public LogBuilder HasApplicationLogFilePath(String applicationLogFilePath)
    {
      this.tokens.Add(ApplicationKey);
      this.tokens.Add(applicationLogFilePath);
      return this;
    }

    public LogBuilder HasJobLogFilePath(String jobLogFilePath)
    {
      this.tokens.Add(JobKey);
      this.tokens.Add(jobLogFilePath);
      return this;
    }
  }
}
