namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;

  public class PhyrexiaManaSource : IManaSource
  {
    private readonly ManaUnit[] _units;

    public PhyrexiaManaSource(Player player)
    {
      Owner = player;

      _units = new[]
      {
        new ManaUnit(new ManaColor(isWhite: true, isBlue: true, isBlack: true, isRed: true, isGreen: true, isPhyrexian: true), 
          rank: 100, source: this)
      };
    }

    public bool CanActivate()
    {
      return Owner.Life > 2;
    }

    public void PayActivationCost()
    {
      Owner.Life -= 2;
    }

    public Card OwningCard { get; private set; }
    public Player Owner { get; private set; }

    public IEnumerable<ManaUnit> GetUnits()
    {
      return _units;
    }
  }
}
