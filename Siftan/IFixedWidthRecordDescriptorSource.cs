
namespace Siftan
{
  using System;

  /// <summary>
  /// Provides a fixed width descriptor.
  /// </summary>
  public interface IFixedWidthRecordDescriptorSource
  {
    /// <summary>
    /// Returns true if a fixed width record can be returned from this source; false otherwise.
    /// </summary>
    Boolean HasFixedWidthRecord { get; }

    /// <summary>
    /// Gets a fixed width record from the source.
    /// </summary>
    /// <returns>Returns fixed width record.</returns>
    FixedWidthRecordDescriptor GetFixedWidthRecord();
  }
}
