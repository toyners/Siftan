
namespace Siftan.TestSupport
{
  using System;
  using System.IO;

  public class AcceptanceTestsBase
  {
    protected String workingDirectory = null;

    protected String inputFilePath = null;

    protected String matchedOutputFilePath = null;

    protected String unmatchedOutputFilePath = null;

    protected String applicationLogFilePath = null;

    protected String jobLogFilePath = null;

    protected void SetFilePaths(String projectName, String extension)
    {
      this.workingDirectory = Path.GetTempPath() + projectName + @"\";
      this.inputFilePath = this.workingDirectory + "Input." + extension;
      this.matchedOutputFilePath = this.workingDirectory + "Matched." + extension;
      this.unmatchedOutputFilePath = this.workingDirectory + "Unmatched." + extension;
      this.applicationLogFilePath = this.workingDirectory + "Application.log";
      this.jobLogFilePath = this.workingDirectory + "Job.log";
    }

    protected void SetFilePathsForDelimitedJob(String projectName)
    {
      this.SetFilePaths(projectName, "csv");
    }

    protected void CreateEmptyWorkingDirectory()
    {
      if (Directory.Exists(this.workingDirectory))
      {
        Directory.Delete(this.workingDirectory, true);
      }

      Directory.CreateDirectory(this.workingDirectory);
    }
  }
}
