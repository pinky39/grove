namespace Grove.Infrastructure
{
  using System;
  using System.Linq;

  public static class StringUtils
  {
    public static string Capitalize(this string str)
    {
      var chars = str.ToCharArray();
      chars[0] = Char.ToUpper(chars[0]);
      return new string(chars);
    }

    public static string CapitalizeEachWord(this string str)
    {
      var words = str.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Capitalize());

      return String.Join(" ", words);
    }

    public static string ToPascalCase(this string sentence)
    {
      var words = sentence.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Capitalize());

      return String.Join("", words);
    }

    public static string UnCapitalize(this string str)
    {
      var chars = str.ToCharArray();
      chars[0] = Char.ToLower(chars[0]);
      return new string(chars);
    }
  }
}