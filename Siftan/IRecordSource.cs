
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Represents a source of records in a list structure.
  /// </summary>
  public interface IRecordSource : IDisposable
  {
    /// <summary>
    /// Gets a value indicating whether there is a selected record.
    /// </summary>
    Boolean GotRecord { get; }

    /// <summary>
    /// Close the record source. Any underlying resources will be closed.
    /// </summary>
    void Close();

    /// <summary>
    /// Gets the data of the currently selected record from within the record source.
    /// </summary>
    /// <param name="buffer">Buffer to write the record data to.</param>
    /// <returns>Number of bytes written to the buffer. 0 if there is no more record data to read.</returns>
    Int32 GetRecordData(Byte[] buffer);

    /// <summary>
    /// Gets the data tags for the currently selected record from within the record source.
    /// </summary>
    /// <param name="tags">Dictionary to hold the data tags. Dictionary will be cleared before data tags
    /// are inserted. Keys start at "tag1"</param>
    /// <returns>Number of data tags associated with the record.</returns>
    Int32 GetTagData(Dictionary<String, String> tags);

    /// <summary>
    /// Move to the next record in the source.
    /// </summary>
    /// <returns>True if the move was successful. False if there are no more records in the source. GotRecord will reflect this result.</returns>
    Boolean MoveToNextRecord();
  }
}
