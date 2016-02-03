
namespace Siftan.TestSupport
{
  using System;
  using System.Collections.Generic;

  public class InputBuilder : IBuilder
  {
    public const String SingleFile = @"C:\Directory\File.txt";

    public const String MultipleFiles = @"C:\Directory\*.txt";

    public const String SearchSubDirectories = "-r";

    private List<String> tokens = new List<String>();

    public String[] Build()
    {
      return this.tokens.ToArray();
    }

    public InputBuilder IsSingleFile(String filePath = SingleFile)
    {
      this.tokens.Add(filePath);
      return this;
    }

    public InputBuilder IsMultipleFiles(String filePattern = MultipleFiles)
    {
      this.tokens.Add(filePattern);
      return this;
    }

    public InputBuilder AndSearchSubDirectories()
    {
      this.tokens.Add(SearchSubDirectories);
      return this;
    }
  }
}
