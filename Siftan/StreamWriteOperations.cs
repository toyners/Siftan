
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  public static class StreamWriteOperations
  {
    public static void WriteRecordToStream(StreamWriter writer, IStreamReader reader, Record record)
    {
      Int64 position = reader.Position;

      reader.Position = record.Start;
      while (reader.Position < record.End)
      {
        writer.WriteLine(reader.ReadLine());
      }

      reader.Position = position;
    }
  }
}
