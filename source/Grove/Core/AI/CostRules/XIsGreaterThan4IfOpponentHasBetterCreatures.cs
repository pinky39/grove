namespace Grove.AI.CostRules
{
  using System.Linq;

  public class XIsGreaterThan4IfOpponentHasBetterCreatures : CostRule
  {
    private readonly int _x;

    private XIsGreaterThan4IfOpponentHasBetterCreatures() {}

    public XIsGreaterThan4IfOpponentHasBetterCreatures(int x)
    {
      _x = x;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var maxX = p.Controller.GetAvailableManaCount(ManaUsage.Spells, p.OwningCard.Has().Convoke, p.OwningCard.Has().Delve) 
        - p.OwningCard.ManaCost.Converted;

      if (maxX >= _x)
      {
        var yourScore = p.Controller.Battlefield.Creatures.Sum(x => x.Score);
        var opponentScore = p.Controller.Opponent.Battlefield.Creatures.Sum(x => x.Score);

        return opponentScore >= yourScore ? maxX : 4;
      }

      return maxX;
    }
  }
}