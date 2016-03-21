
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Text;

  public class DelimitedRecordSource : IRecordSource
  {
    private readonly DelimitedRecordDescriptor descriptor;
    private readonly List<Int64> positions;
    private FileReader file;

    private String endofLineDelimiter = "\r\n";

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath)
    {
      this.descriptor = descriptor;
      this.file = new FileReader(filePath, "\r\n");
      this.positions = new List<Int64>();

      this.ReadRecord();
    }

    public Int64 GetRecordCount
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Boolean GotRecord { get; private set; }

    public void Close()
    {
      this.file.Close();
    }

    public Boolean GetRecordData(Byte[] buffer, out Int64 bytesRead)
    {
      throw new NotImplementedException();
    }

    public Boolean MoveToNextRecord()
    {
      if (this.GotRecord)
      {
        this.ReadRecord();
      }

      return this.GotRecord;
    }

    public Boolean MoveToRecord(Int64 index)
    {
      throw new NotImplementedException();
    }

    private void ReadRecord()
    {
      StringBuilder termBuilder = new StringBuilder(1024);
      Char character = '\0';
      Int32 delimiterIndex = 0;
      Int32 termIndex = 0;
      Int32 eolIndex = 0;
      Int64 recordPosition = this.file.Position;
      this.GotRecord = false;

      while (true)
      {
        if (!this.file.TryGetNextCharacter(ref character))
        {
          break;
        }

        if (GotEndOfLine(character, ref eolIndex))
        {

        }

        if (GotTerm(character, ref delimiterIndex))
        {
          // Got the term to examine
          if (termIndex == this.descriptor.LineIDIndex && this.descriptor.HeaderID == termBuilder.ToString())
          {
            // Got the record header - mark the position down
            this.positions.Add(recordPosition);
            this.GotRecord = true;
          }

          termBuilder.Clear();
          continue;
        }
        
        termBuilder.Append(character);
      }
    }

    private Boolean GotEndOfLine(Char character, ref Int32 eolIndex)
    {
      throw new NotImplementedException();
    }

    private Boolean GotTerm(Char character, ref Int32 delimiterIndex)
    {
      if (character == this.descriptor.Delimiter[delimiterIndex])
      {
        if (delimiterIndex + 1 == this.descriptor.Delimiter.Length)
        {
          delimiterIndex = 0;
          return true;
        }

        delimiterIndex++;
      }
      else
      {
        delimiterIndex = 0;
      }

      return false;
    }
  }

  public class FileReader
  {
    private FileStream stream;
    private Byte[] buffer;
    private Int32 bufferIndex;
    private Int32 bufferLength;
    private String endOfLineDelimiter;

    public Int64 Position { get { return this.stream.Position; } }

    public FileReader(String filePath, String endOfLineDelimiter)
    {
      this.buffer = new Byte[1024];
      this.bufferIndex = this.bufferLength = 0;
      this.stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

      this.stream.ReadByte();
      this.stream.ReadByte();
      this.stream.ReadByte();
    }

    public void Close()
    {
      if (this.stream != null)
      {
        this.stream.Close();
        this.stream = null;
      }
    }

    public Boolean TryGetNextCharacter(ref Char character)
    {
      if (this.bufferIndex == this.bufferLength)
      {
        this.bufferIndex = 0;
        this.bufferLength = this.stream.Read(this.buffer, 0, this.buffer.Length);
        if (this.bufferLength == 0)
        {
          return false; // Nothing read must be EOF
        }
      }

      character = (Char)this.buffer[this.bufferIndex++];
      return true;
    }
  }
}
