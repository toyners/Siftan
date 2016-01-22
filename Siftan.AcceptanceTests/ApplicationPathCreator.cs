
namespace Siftan.AcceptanceTests
{
  using System;
  using System.IO;
  using NUnit.Framework;

  public static class ApplicationPathCreator
  {
    public static String GetApplicationPath(String applicationName)
    {
      const String ApplicationPathTemplate = @"C:\C#\Siftan\{0}\bin\{1}\{0}.exe";

      var applicationPath = String.Format(ApplicationPathTemplate,
        applicationName,
        (TestContext.CurrentContext.TestDirectory.Contains("Release") ? "Release" : "Debug"));

      VerifyApplicationExists(applicationPath);

      return applicationPath;
    }

    private static void VerifyApplicationExists(String applicationPath)
    {
      if (!File.Exists(applicationPath))
      {
        throw new FileNotFoundException(String.Format("File '{0}' not found.", applicationPath));
      }
    }
  }
}
