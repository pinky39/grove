namespace Grove.Infrastructure
{
  using System.Diagnostics;
  using System.Reflection;

  public class AssemblyEx
  {
    public static string ProductVersion
    {
      get
      {
        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
      }
    }
  }
}