
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class CommandLineArgumentsBuilder : IBuilder
  {
    private List<IBuilder> builders = new List<IBuilder>();

    public CommandLineArgumentsBuilder WithInput(InputBuilder inputBuilder)
    {
      this.builders.Add(inputBuilder);
      return this;
    }

    public CommandLineArgumentsBuilder WithDelim(DelimBuilder delimBuilder)
    {
      this.builders.Add(delimBuilder);
      return this;
    }

    public CommandLineArgumentsBuilder WithInList(InListBuilder inListBuilder)
    {
      this.builders.Add(inListBuilder);
      return this;
    }

    public CommandLineArgumentsBuilder WithOutput(OutputBuilder outputBuilder)
    {
      this.builders.Add(outputBuilder);
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
