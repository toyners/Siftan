
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Validation;

  public class FileReader : IStreamReader
  {
    #region Fields
    private Byte[] lineTerminator = new Byte[] { (Byte)'\r', (Byte)'\n' };

    private Int32 blockIndex;

    private Int32 blockSize = -1;

    private Byte[] buffer;

    private Boolean disposed;

    private Int64 position;

    private FileStream stream;
    #endregion

    #region Construction
    public FileReader(String path) : this(path, 8192)
    {
    }

    public FileReader(String path, Int32 bufferSize)
    {
      path.VerifyThatStringIsNotNullAndNotEmpty("Parameter 'path' is null or empty.");
      if (bufferSize <= 0)
      {
        throw new Exception("Parameter 'bufferSize' must be a positive non-zero integer");
      }

      this.stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.RandomAccess);
      this.buffer = new Byte[bufferSize];
    }
    #endregion

    #region Properties
    public Boolean EndOfStream
    {
      get 
      {
        return this.stream.Position == this.stream.Length && this.BlockIsEmpty;
      }
    }

    public Int64 Position
    {
      get 
      {
        return this.position;
      }

      set 
      {
        Int64 difference = value - this.position;
        if (Math.Abs(difference) > this.blockSize)
        {
          this.blockSize = -1;
          this.position = this.stream.Position = value;
          return;
        }

        this.blockIndex += (Int32)difference;
        if (this.blockIndex < 0 || this.blockIndex >= this.blockSize)
        {
          this.blockSize = -1;
          this.position = this.stream.Position = value;
          return;
        }


        this.position += (Int32)difference;
      }
    }

    public Int64 Length
    {
      get
      {
        return this.stream.Length;
      }
    }

    private Boolean BlockIsEmpty 
    { 
      get 
      { 
        return this.blockSize == -1 || this.blockIndex >= this.blockSize; 
      }
    }
    #endregion

    #region Methods
    public String ReadLine()
    { 
      Byte nextByte;
      StringBuilder builder = new StringBuilder(100);
      Int32 lineTerminatorIndex = 0;

      while (this.TryGetNextByte(out nextByte))
      {
        if (nextByte != lineTerminator[lineTerminatorIndex])
        {
          builder.Append((Char)nextByte);
          continue;
        }

        lineTerminatorIndex++;
        if (lineTerminatorIndex == lineTerminator.Length)
        {
          this.position += lineTerminator.Length;
          break;
        }
      }

      if (builder.Length != 0)
      {
        this.position += builder.Length;
        return builder.ToString();
      }

      return null;
    }

    private Boolean TryGetNextByte(out Byte nextByte)
    {
      if (this.EndOfStream)
      {
        nextByte = 0;
        return false;
      }

      if (this.BlockIsEmpty)
      {
        this.ReadNextBlock();
      }

      nextByte = this.buffer[this.blockIndex++];
      return true;
    }

    private void ReadNextBlock()
    {
      this.blockSize = this.stream.Read(this.buffer, 0, this.buffer.Length);
      this.blockIndex = 0;
    }

    public void Close()
    {
      this.Dispose(true);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(Boolean disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (disposing)
      {
        this.buffer = null;
        if (this.stream != null)
        {
          this.stream.Dispose();
          this.stream = null;
        }
      }

      this.disposed = true;
    }
    #endregion
  }
}
