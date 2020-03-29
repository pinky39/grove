namespace Grove.Infrastructure
{
  using System.Linq;
  using System.Security.Cryptography;
  using System.Text;

  public static class Sha1
  {
    public static string Calculate(string input)
    {
      using (SHA1Managed sha1 = new SHA1Managed())
      {
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder(hash.Length * 2);

        foreach (byte b in hash)
        {
          // can be "x2" if you want lowercase
          sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
      }
    }
  }
}