
namespace Siftan.WinForms.AcceptanceTests
{
  using System;
  using System.Reflection;
  using Jabberwocky.Toolkit.Assembly;

  public static class InputFileCreator
  {
    public static void CreateFile(String embeddedResourcePath, String filePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(embeddedResourcePath, filePath);
    }
  }
}
