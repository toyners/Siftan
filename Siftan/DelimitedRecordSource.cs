
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
    private const Byte EF = 239;
    private const Byte BB = 187;
    private const Byte BF = 191;

    private readonly DelimitedRecordDescriptor descriptor;
    private readonly FileStream stream;
    private readonly List<Int64> positions;
    private Byte[] buffer;
    private Int32 bufferIndex;
    private Int32 bufferLength;
    private Encoding encoding;
    private Int32 byteCount; // Number of bytes per character

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath) :
      this(descriptor, filePath, Encoding.UTF8)
    {
    }

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath, Encoding encoding)
    {
      this.descriptor = descriptor;
      this.stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

      this.DetermineEncoding(encoding);
      
      this.buffer = new Byte[1024 * this.byteCount];
      this.bufferIndex = this.bufferLength = 0;

      this.ReadRecord();
    }

    private void DetermineEncoding(Encoding encoding)
    {
      if (encoding != null)
      {
        this.encoding = encoding;

        this.stream.ReadByte();
        this.stream.ReadByte();
        this.stream.ReadByte();
      }
      else
      {
        // Determine encoding from BOM. Use UTF8 if no BOM are found.
        if (this.stream.ReadByte() == EF &&
          this.stream.ReadByte() == BB &&
          this.stream.ReadByte() == BF)
        {
          this.encoding = Encoding.UTF8;
        }
      }

      this.byteCount = 1; // this.encoding.GetMaxByteCount(1);
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
      throw new NotImplementedException();
    }

    public Boolean MoveToRecord(Int64 index)
    {
      throw new NotImplementedException();
    }

    private void ReadRecord()
    {
      //Boolean withinQualifier = false;
      StringBuilder stringBuilder = new StringBuilder(1024);
      Char character = '\0';

      while (true)
      {
        if (!TryGetNextCharacter(ref character))
        {
          break;
        }
        
        stringBuilder.Append(character);
      }
    }

    private Boolean TryGetNextCharacter(ref Char character)
    {
      if (this.bufferIndex == this.bufferLength)
      {
        this.bufferLength = this.stream.Read(this.buffer, 0, this.buffer.Length);
        if (this.bufferLength == 0)
        {
          return false; // Nothing read must be EOF
        }

        this.bufferIndex = 0;
      }

      character = this.encoding.GetChars(this.buffer, this.bufferIndex, this.byteCount)[0];
      return true;
    }
  }
}
