namespace Grove
{
  using System.Collections.Generic;

  public class DelveManaSource : IManaSource
  {
    private readonly ManaUnit[] _units;

    public DelveManaSource(Card card)
    {
      OwningCard = card;
      _units = new[]
        {
          new ManaUnit(ManaColor.Colorless, rank: 100, source: this)
        };
    }

    public bool CanActivate()
    {
      return true;
    }

    public void PayActivationCost()
    {
      OwningCard.Exile();
    }

    public Card OwningCard { get; private set; }

    public IEnumerable<ManaUnit> GetUnits()
    {
      return _units;
    }
  }
}
