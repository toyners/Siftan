
namespace Siftan_Console
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Jabberwocky.Toolkit.IO;
  using Siftan;

  public class Program
  {
    public static void Main(String[] args)
    {
      FileReader fileReader = new FileReader(@"C:\C#\Siftan\testdata.txt");

      DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        HeaderID = "01",
        LineIDIndex = 0
      };

      DelimitedRecordReader delimitedReader = new DelimitedRecordReader(recordDescriptor);

      InListExpression expression = new InListExpression(new []{ "12345" });

      OneFileRecordWriter recordWriter = new OneFileRecordWriter(@"C:\C#\Siftan\Test output\matched.txt", @"C:\C#\Siftan\Test output\unmatched.txt");

      Engine engine = new Engine();

      engine.Execute(new[] { @"C:\C#\Siftan\Testdata.txt" }, null, delimitedReader, expression, recordWriter.WriteMatchedRecord, recordWriter.WriteUnmatchedRecord);
    }
  }
}
