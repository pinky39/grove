namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public interface IHashable
  {
    int CalculateHash(HashCalculator calc);
  }

  public interface IHashDependancy
  {
    void InvalidateHash();
  }

  public class NoHashDependency : IHashDependancy
  {
    public void InvalidateHash() { }
  }

  public class HashCalculator
  {
    private readonly Dictionary<object, int> _hashCache = new Dictionary<object, int>();

    public int Calculate(IHashable hashable)
    {
      var hash = 0;

      if (hashable == null)
        return hash;

      if (_hashCache.TryGetValue(hashable, out hash) == false)
      {
        hash = hashable.CalculateHash(this);
        _hashCache.Add(hashable, hash);
      }

      return hash;
    }


    public static int Combine(IEnumerable<int> values)
    {
      uint h = 0;

      foreach (var value in values)
      {
        Hash(ref h, value);
      }

      return Avalanche(h);
    }


    public static int Combine(params int[] values)
    {
      return Combine((IEnumerable<int>) values);
    }

    public static int CombineCommutative(params int[] values)
    {
      return CombineCommutative((IList<int>) values);
    }

    public static int CombineCommutative(IList<int> values)
    {
      var h = values.Count;

      foreach (var value in values)
      {
        h = h + value;
      }

      return Avalanche((uint)h);
    }

    private static unsafe int Avalanche(uint h)
    {
      h += (h << 3);
      h ^= (h >> 11);
      h += (h << 15);
      return *((int*) (void*) &h);
    }

    private static unsafe void Hash(ref uint h, int data)
    {
      var d = (byte*) &data;
      Hash(d, sizeof (int), ref h);
    }

    private static unsafe void Hash(byte* d, int len, ref uint h)
    {
      for (var i = 0; i < len; i++)
      {
        h += d[i];
        h += (h << 10);
        h ^= (h >> 6);
      }
    }
  }
}