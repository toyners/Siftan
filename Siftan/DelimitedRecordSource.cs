
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
    private String endOfLineDelimiter = "\r\n";
    private Boolean onNextRecord = false;
    private Int64 recordPosition;

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath)
    {
      this.descriptor = descriptor;
      this.file = new FileReader(filePath);
      this.positions = new List<Int64>();
      this.recordPosition = this.file.Position;
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
      this.GotRecord = false;

      if (this.onNextRecord)
      {
        this.positions.Add(this.recordPosition);
        this.GotRecord = true;
        this.onNextRecord = false;
      }

      while (true)
      {
        if (!this.file.TryGetNextCharacter(ref character))
        {
          break;
        }

        if (GotDelimiter(character, this.endOfLineDelimiter, ref eolIndex))
        {
          termIndex = 0;
          termBuilder.Clear();
          recordPosition = this.file.Position;
          continue;
        }

        if (GotDelimiter(character, this.descriptor.Delimiter, ref delimiterIndex))
        {
          // Got the term to examine
          if (this.GotTerm(termIndex, termBuilder.ToString()))
          {
            if (!this.GotRecord)
            {
              // Got the first record header - mark the position down
              this.positions.Add(recordPosition);
              this.GotRecord = true;
            }
            else
            {
              this.onNextRecord = true;
              break;
            }
          }

          termBuilder.Clear();
          continue;
        }
        
        termBuilder.Append(character);
      }
    }

    private Boolean GotTerm(Int32 termIndex, String term)
    {
      return termIndex == this.descriptor.LineIDIndex && term == this.descriptor.HeaderID;
    }

    private Boolean GotDelimiter(Char character, String delimiter, ref Int32 delimiterIndex)
    {
      if (character == delimiter[delimiterIndex])
      {
        if (delimiterIndex + 1 == delimiter.Length)
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

    public Int64 Position { get; set; }

    public FileReader(String filePath)
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
      this.Position++;
      return true;
    }
  }
}
