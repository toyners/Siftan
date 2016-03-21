
namespace Siftan
{
  using System;
  using System.IO;

  public class FileReader
  {
    #region Fields
    private FileStream stream;
    private Byte[] buffer;
    private Int32 bufferIndex;
    private Int32 bufferLength;
    #endregion

    #region Construction
    public FileReader(String filePath)
    {
      this.buffer = new Byte[1024];
      this.bufferIndex = this.bufferLength = 0;
      this.stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

      this.stream.ReadByte();
      this.stream.ReadByte();
      this.stream.ReadByte();
    }
    #endregion

    #region Properties
    public Int64 Position { get; set; }
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
    #endregion
  }
}
