
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class DelimitedRecordSource : IRecordSource
  {
    private readonly DelimitedRecordDescriptor descriptor;
    private readonly FileStream stream;
    private readonly List<Int64> positions;
    private Byte[] buffer;
    private Int32 bufferIndex;
    private Int32 bufferLength;

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath)
    {
      this.descriptor = descriptor;
      this.stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      this.buffer = new Byte[1024];
      this.bufferIndex = this.bufferLength = 0;
      this.positions = new List<Int64>();

      this.stream.ReadByte();
      this.stream.ReadByte();
      this.stream.ReadByte();

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
      throw new NotImplementedException();
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
      StringBuilder stringBuilder = new StringBuilder(1024);
      Char character = '\0';
      Int32 delimiterIndex = 0;
      Int32 termIndex = 0;
      Int64 recordPosition = this.stream.Position;

      while (true)
      {
        if (!TryGetNextCharacter(ref character))
        {
          break;
        }

        if (character == this.descriptor.Delimiter[delimiterIndex])
        {
          if (++delimiterIndex == this.descriptor.Delimiter.Length)
          {
            // Got the term to examine
            if (termIndex == this.descriptor.LineIDIndex)
            {
              if (this.descriptor.HeaderID == stringBuilder.ToString())
              {
                // Got the record header - mark the position down
                this.positions.Add(recordPosition);
                this.GotRecord = true;
              }
            }

            stringBuilder.Clear();
            delimiterIndex = 0;
          }

          continue;
        }

        
        stringBuilder.Append(character);
      }
    }

    private Boolean TryGetNextCharacter(ref Char character)
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
