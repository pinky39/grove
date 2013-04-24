namespace Grove.Ai.CostRules
{
  public class ReduceToughnessOfEachCreature : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      const int maxXToTry = 6;

      var yourCreatures = p.Controller.Battlefield.Creatures;
      var opponentCreatures = p.Controller.Opponent.Battlefield.Creatures;

      var score = new int[maxXToTry];

      foreach (var creature in opponentCreatures)
      {
        if (creature.Life <= maxXToTry)
          score[creature.Life - 1]++;
      }

      foreach (var creature in yourCreatures)
      {
        if (creature.Life <= maxXToTry)
          score[creature.Life - 1]--;
      }

      for (var i = 1; i < maxXToTry; i++)
      {
        score[i] += score[i - 1];
      }

      var result = int.MaxValue;
      var best = 0;

      for (var i = 0; i < maxXToTry; i++)
      {
        if (score[i] > best)
        {
          best = score[i];
          result = i + 1;
        }
      }

      return result;
    }
  }
}