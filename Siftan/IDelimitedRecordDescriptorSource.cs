
namespace Siftan
{
  using System;

  /// <summary>
  /// Provides a delimited record descriptor.
  /// </summary>
  public interface IDelimitedRecordDescriptorSource
  {
    /// <summary>
    /// Returns true if a delimited record can be returned from this source; false otherwise.
    /// </summary>
    Boolean HasDelimitedRecord { get; }

    /// <summary>
    /// Gets a delimited record from the source.
    /// </summary>
    /// <returns>Returns delimited record.</returns>
    DelimitedRecordDescriptor GetDelimitedRecord();
  }
}
