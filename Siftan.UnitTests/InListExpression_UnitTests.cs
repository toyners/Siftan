
namespace Siftan.UnitTests
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class InListExpression_UnitTests
  {
    [Test]
    [TestCase("A", true)]
    [TestCase("C", false)]
    public void IsMatch_DifferentScenarios_ReturnsExpectedResult(String term, Boolean expectedResult)
    {
      // Arrange
      InListExpression expression = new InListExpression(new[] { "A", "B" });
      Record record = new Record
      {
        Term = term
      };

      // Act
      Boolean result = expression.IsMatch(record);

      // Assert
      result.Should().Be(expectedResult);
    }

    [Test]
    public void HasReachedMatchQuota_SetToFirstMatchInList_ReturnsTrueAfterFirstMatch()
    {
      InListExpression expression = new InListExpression(new[] { "A", "B" }, InListExpression.MatchQuotas.FirstMatchInList);
      Record record = new Record
      {
        Term = "A"
      };

      // Act
      expression.IsMatch(record);

      // Assert
      expression.HasReachedMatchQuota.Should().BeTrue();
    }

    [Test]
    public void HasReachedMatchQuota_SetToFirstMatchOfEachInList_ReturnsTrueAfterAllTermsAreMatched()
    {
      InListExpression expression = new InListExpression(new[] { "A", "B", "C" }, InListExpression.MatchQuotas.FirstMatchOfEachTermInList);

      // Act
      expression.IsMatch(new Record { Term = "A" });
      Boolean firstResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "B" });
      Boolean secondResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "A" });
      Boolean thirdResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "C" });
      Boolean lastResult = expression.HasReachedMatchQuota;

      // Assert
      firstResult.Should().BeFalse();
      secondResult.Should().BeFalse();
      firstResult.Should().BeFalse();
      lastResult.Should().BeTrue();
    }

    [Test]
    public void HasReachedMatchQuota_SetToNone_ReturnsFalseAfterAllMatches()
    {
      InListExpression expression = new InListExpression(new[] { "A", "B", "C" });

      // Act
      expression.IsMatch(new Record { Term = "A" });
      Boolean firstResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "B" });
      Boolean secondResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "A" });
      Boolean thirdResult = expression.HasReachedMatchQuota;
      expression.IsMatch(new Record { Term = "C" });
      Boolean lastResult = expression.HasReachedMatchQuota;

      // Assert
      firstResult.Should().BeFalse();
      secondResult.Should().BeFalse();
      firstResult.Should().BeFalse();
      lastResult.Should().BeFalse();
    }
  }
}
