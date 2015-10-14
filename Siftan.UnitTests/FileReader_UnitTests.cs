
namespace Siftan.UnitTests
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class FileReader_UnitTests
  {
    #region Methods
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void FileReader_BufferSizeIsNotPositive_ThrowsMeaningfulException(Int32 bufferSize)
    {
      Action action = () => new FileReader("Path", bufferSize);

      action.ShouldThrow<Exception>().WithMessage("Parameter 'bufferSize' must be a positive non-zero integer");
    }
    #endregion
  }
}
