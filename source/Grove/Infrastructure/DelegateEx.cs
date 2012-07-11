namespace Grove.Infrastructure
{
  using System;
  using System.Text;
  using System.Text.RegularExpressions;

  public class DelegateEx
  {
    private static readonly Regex ClassName1 = new Regex(@".*\.(.*)\+<>c__");
    private static readonly Regex ClassName2 = new Regex(@".*\.(.*)$");
    private static readonly Regex MethodName = new Regex(@".*<(.*)>b__");

    public static string GetNameOfAnonymousFuncOwner(Delegate del)
    {
      var result = new StringBuilder();

      if (del.Target != null)
      {
        result.AppendFormat("{0}.", GetClassName(del.Target.ToString()));
      }

      result.AppendFormat(GetMethodName(del.Method.ToString()));

      return result.ToString();
    }

    private static string GetClassName(string generatedClassName)
    {
      var match = ClassName1.Match(generatedClassName);
      if (match.Success)
      {
        return match.Groups[1].Value;
      }

      match = ClassName2.Match(generatedClassName);
      if (match.Success)
      {
        return match.Groups[1].Value;
      }

      return generatedClassName;
    }

    private static string GetMethodName(string generatedMethodName)
    {
      var match = MethodName.Match(generatedMethodName);
      if (match.Success)
      {
        return match.Groups[1].Value;
      }
      return generatedMethodName;
    }
  }
}