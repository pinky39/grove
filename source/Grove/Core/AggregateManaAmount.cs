namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class AggregateManaAmount : IManaAmount
  {
    private readonly List<IManaAmount> _amounts = new List<IManaAmount>(10);
    private int? _converted;
    
    public AggregateManaAmount(params IManaAmount[] amounts)
    {
      _amounts.AddRange(amounts);      
    }
    
    public AggregateManaAmount(IEnumerable<IManaAmount> amounts)
    {
      _amounts.AddRange(amounts);      
    }
        
    public IEnumerator<Mana> GetEnumerator()
    {
      return _amounts.SelectMany(amount => amount).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted
    {
      get { return _converted ?? (_converted = _amounts.Sum(x => x.Converted)).Value; }
    }
  }
}