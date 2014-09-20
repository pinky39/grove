namespace Grove.Tests.Unit
{
  using Grove.Infrastructure;
  using Xunit;

  public class StringExFacts
  {
    [Fact]
    public void ToPascalCase()
    {
      const string sentence = "Tap to add";
      const string pascalCase = "TapToAdd";

      Assert.Equal(pascalCase, sentence.ToPascalCase());
    }
  }
}