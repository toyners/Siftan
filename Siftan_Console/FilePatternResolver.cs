
namespace Siftan_Console
{
  using System;
  using System.IO;

  public class FilePatternResolver
  {
    public enum SearchDepths
    {
      AllDirectories,
      InitialDirectoryOnly
    }

    public String[] ResolveFilePattern(String filePattern, SearchDepths searchDepth)
    {
      SearchOption searchOption = (searchDepth == SearchDepths.InitialDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);

      String path = Path.GetDirectoryName(filePattern);
      String searchPattern = Path.GetFileName(filePattern);

      return Directory.GetFiles(path, searchPattern, searchOption);
    }
  }
}
