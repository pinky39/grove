namespace Grove.Infrastructure
{
  using System.Collections;
  using System.Collections.Generic;
  using Core;

  public class HashCalculator
  {
    private readonly Dictionary<object, int> _hashCache = new Dictionary<object, int>();

    public int Calculate(params object[] objects)
    {
      return Calculate((IEnumerable) objects);
    }    

    // source 
    // http://stackoverflow.com/questions/1079192/is-it-possible-to-combine-hash-codes-for-private-members-to-generate-a-new-hash
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

    private void Append(ref uint h, object obj)
    {
      if (obj == null)
        return;

      int hashcode;
      var hashable = obj as IHashable;
      var collection = obj as IEnumerable;

      if (hashable != null)
      {
        if (_hashCache.TryGetValue(obj, out hashcode) == false)
        {
          hashcode = hashable.CalculateHash(this);
          _hashCache.Add(obj, hashcode);
        }
      }
      else if (collection != null)
      {
        hashcode = Calculate(collection);
      }
      else
      {
        hashcode = obj.GetHashCode();
      }

      Hash(ref h, hashcode);
    }

    private int Calculate(IEnumerable objects)
    {
      uint h = 0;
      
      foreach (var item in objects)
      {
        Append(ref h, item);
      }

      return Avalanche(h);
    }
  }
}