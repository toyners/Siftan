
namespace Siftan.TestSupport
{
  using System;
  using System.IO;

  /// <summary>
  ///  Provides methods for directories used in tests.
  /// </summary>
  public static class TestDirectory
  {
    /// <summary>
    /// Ensure that the directory exists and is cleared of any files and sub-directories.
    /// </summary>
    /// <param name="path">Full path of directory to be created or cleared.</param>
    public static void ClearDirectory(String path)
    {
      if (Directory.Exists(path))
      {
        Directory.Delete(path, true);
      }

      Directory.CreateDirectory(path);
    }
  }
}
