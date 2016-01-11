
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class CommandLineArgumentsBuilder : IBuilder
  {
    private List<IBuilder> builders = new List<IBuilder>();

    public CommandLineArgumentsBuilder WithInput(InputBuilder inputBuilder)
    {
      builders.Add(inputBuilder);
      return this;
    }

    public String[] Build()
    {
      List<String> results = new List<String>();

      foreach (IBuilder builder in this.builders)
      {
        results.AddRange(builder.Build());
      }

      return results.ToArray();
    }
  }
}
