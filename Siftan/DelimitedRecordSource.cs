﻿
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class DelimitedRecordSource : IRecordSource
  {
    private readonly DelimitedRecordDescriptor descriptor;
    private readonly List<Int64> positions;
    private FileReader file;
    private String endOfLineDelimiter = "\r\n";
    private Boolean onNextRecord = false;
    private Int64 recordPosition;
    private Int32 recordIndex = 0;
    private Int32 recordLength = -1;
    private Boolean disposed;

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath)
    {
      this.descriptor = descriptor;
      this.file = new FileReader(filePath);
      this.positions = new List<Int64>();
      this.recordPosition = this.file.Position;
      this.ReadRecord();
    }

    public Boolean GotRecord { get; private set; }

    public void Close()
    {
      this.Dispose(true);
    }

    /// <summary>
    /// Performs tasks to release unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    public Int32 GetRecordData(Byte[] buffer)
    {
      if (this.recordLength == -1)
      {
        Int64 position = this.positions[this.recordIndex];
        Int64 length = 0;

        if (this.recordIndex == this.positions.Count - 1)
        {
          length = this.recordPosition - position;
        }
        else
        {
          length = this.positions[this.recordIndex + 1] - position;
        }

        if (length > Int32.MaxValue)
        {
          throw new Exception();
        }

        this.recordLength = (Int32)length;

        this.file.Position = position;
      }

      if (this.recordLength > buffer.Length)
      {
        this.recordLength -= buffer.Length;
        return this.file.ReadBuffer(buffer, buffer.Length);
      }
      else
      {
        var bytesRead = this.file.ReadBuffer(buffer, this.recordLength);
        this.recordLength = -1;
        return bytesRead;
      }
    }

    public Int32 GetTagData(Dictionary<String, String> tags)
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

    protected virtual void Dispose(Boolean disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (disposing)
      {
        if (this.file != null)
        {
          this.file.Close();
          this.file = null;
        }
      }

      this.disposed = true;
    }

    private void ReadRecord()
    {
      var termBuilder = new StringBuilder(1024);
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
          this.recordPosition = this.file.Position;
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
              this.positions.Add(this.recordPosition);
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
}
