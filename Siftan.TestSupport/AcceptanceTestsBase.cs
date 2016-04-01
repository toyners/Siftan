
namespace Siftan.TestSupport
{
  using System;
  using System.IO;

  /// <summary>
  /// Base class for test classes. Provides fields and helper methods used by tests.
  /// </summary>
  public class AcceptanceTestsBase
  {
    protected String workingDirectory = null;

    protected String inputFilePath = null;

    protected String matchedOutputFilePath = null;

    protected String unmatchedOutputFilePath = null;

    protected String applicationLogFilePath = null;

    protected String jobLogFilePath = null;

    protected void SetFilePathsForDelimitedJob(String projectName)
    {
      this.SetFilePaths(projectName, "csv");
    }

    protected void SetFilePathsForFixedWidthJob(String projectName)
    {
      this.SetFilePaths(projectName, "txt");
    }

    private void SetFilePaths(String projectName, String extension)
    {
      this.workingDirectory = String.Format(@"C:\Projects\Siftan\Temp\{0}\", projectName);
      this.inputFilePath = this.workingDirectory + "Input." + extension;
      this.matchedOutputFilePath = this.workingDirectory + "Matched." + extension;
      this.unmatchedOutputFilePath = this.workingDirectory + "Unmatched." + extension;
      this.applicationLogFilePath = this.workingDirectory + "Application.log";
      this.jobLogFilePath = this.workingDirectory + "Job.log";
    }
  }
}
