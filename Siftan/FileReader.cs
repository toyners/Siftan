
namespace Siftan
{
  using System;
  using System.IO;

  public class FileReader
  {
    #region Fields
    private const Byte EF = 239;
    private const Byte BB = 187;
    private const Byte BF = 191;

    private FileStream stream;
    private readonly Byte[] buffer;
    private Int32 bufferIndex;
    private Int32 bufferLength;
    private Int64 position;
    #endregion

    #region Construction
    public FileReader(String filePath)
    {
      this.buffer = new Byte[1024];
      this.bufferIndex = this.bufferLength = 0;
      this.stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

      if (this.stream.ReadByte() != EF || this.stream.ReadByte() != BB || this.stream.ReadByte() != BF)
      {
        this.stream.Seek(0, SeekOrigin.Begin);
        return;
      }

      // Ensure that the position is set correctly to account for the BOM
      this.position = 3;
    }
    #endregion

    #region Properties
    public Int64 Position
    {
      get
      {
        return this.position;
      }

      set
      {
        Int64 difference = value - this.position;
        if (Math.Abs(difference) > this.bufferLength)
        {
          this.bufferIndex = this.bufferLength = 0;
          this.position = this.stream.Position = value;
          return;
        }

        this.bufferIndex += (Int32)difference;
        if (this.bufferIndex < 0 || this.bufferIndex >= this.bufferLength)
        {
          this.bufferIndex = this.bufferLength = 0;
          this.position = this.stream.Position = value;
          return;
        }

        this.position += (Int32)difference;
      }
    }

    private Boolean BlockIsEmpty
    {
      get
      {
        return this.bufferIndex >= this.bufferLength;
      }
    }
    #endregion

    #region Methods
    public void Close()
    {
      if (this.stream != null)
      {
        this.stream.Close();
        this.stream = null;
      }
    }

    public Int32 ReadBuffer(Byte[] array, Int32 length)
    {
      Int32 arrayIndex = 0;
      if (!this.BlockIsEmpty)
      {
        var count = Math.Min(this.bufferLength - this.bufferIndex, length);
        length -= count;
        Array.Copy(this.buffer, this.bufferIndex, array, 0, count);

        if (length == 0)
        {
          return count;
        }

        arrayIndex += count;
        this.Position += count;
      }

      return this.stream.Read(array, arrayIndex, length);
    }

    public Boolean TryGetNextCharacter(ref Char character)
    {
      if (this.bufferIndex == this.bufferLength && !this.ReadNextBlock())
      {
        return false; // Nothing read must be EOF
      }

      character = (Char)this.buffer[this.bufferIndex++];
      this.position++;
      return true;
    }

    private Boolean ReadNextBlock()
    {
      this.bufferLength = this.stream.Read(this.buffer, 0, this.buffer.Length);
      this.bufferIndex = 0;
      return (this.bufferLength > 0);
    }
    #endregion
  }
}
