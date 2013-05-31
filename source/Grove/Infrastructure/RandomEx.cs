namespace Grove.Infrastructure
{
  using System;
  using System.Security.Cryptography;

  public static class RandomEx
  {
    [ThreadStatic] private static readonly Random Rnd = new Random();


    /// <summary>
    ///   Returns an integer between min and max (inclusive).
    /// </summary>
    /// <param name = "min"></param>
    /// <param name = "max"></param>
    /// <returns></returns>
    public static int NextStrong(int min, int max)
    {
      // from wikipedia http://en.wikipedia.org/wiki/Fisher-Yates
      // For example, the built-in pseudorandom number generator provided by many programming languages and/or libraries may often have only 32 bits of internal state, 
      // which means it can only produce 2^32 different sequences of numbers. If such a generator is used to shuffle a deck of 52 playing cards, it can only ever produce a very small 
      // fraction of the 52! ≈ 2^225.6 possible permutations. It's impossible for a generator with less than 226 bits of internal state to produce all the possible permutations of a 52-card deck. 
      // It has been suggested[citation needed] that confidence that the shuffle is unbiased can only be attained with a generator with more than about 250 bits of state.

      // To overcome this limitation a random 'sequence' is selected using the RNGCryptoServiceProvider each time the Next is called. RNGCryptoServiceProvider could be used directly but since it can
      // only generate random bytes, special care must be taken to generate numbers from specific range, see: http://www.xs4all.nl/~wrb/Articles_2010/Article_WPFRNGCrypto_01.htm
      var rng = new RNGCryptoServiceProvider();
      var seed = new byte[4];
      rng.GetBytes(seed);

      var rand = new Random(BitConverter.ToInt32(seed, 0));
      return rand.Next(min, max + 1);
    }

    public static int Next(int min, int max)
    {
      return Rnd.Next(min, max + 1);
    }

    public static int Next(int max)
    {
      return Rnd.Next(max);
    }

    public static int Next()
    {
      return Rnd.Next();
    }
  }
}