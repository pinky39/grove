namespace Grove.Tests.Sandbox
{
  using System;
  using System.IO;
  using Xunit;

  public class UriFacts
  {
    [Fact]
    public void RelativeUri()
    {            
      var uri = new Uri(Path.GetFullPath(@".\"));            
      Console.WriteLine(uri.ToString());
    }
  }
}