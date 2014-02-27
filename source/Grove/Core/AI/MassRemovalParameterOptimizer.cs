namespace Grove.AI
{
  using System.Linq;

  public class MassRemovalParameterOptimizer
  {
    public static int CalculateOptimalDamage(Player controller, Player opponent, int max)
    {
      var yourCreatures = controller.Battlefield.Creatures;
      var opponentCreatures = opponent.Battlefield.Creatures;

      var score = new int[max];

      foreach (var creature in opponentCreatures.Where(x => x.Life > 0 && x.Life <= max))
      {        
        score[creature.Life - 1]++;
      }

      foreach (var creature in yourCreatures.Where(x => x.Life > 0 && x.Life <= max))
      {        
        score[creature.Life - 1]--;
      }

      for (var i = 1; i < max; i++)
      {
        score[i] += score[i - 1];
      }

      var result = int.MaxValue;
      var best = 0;

      for (var i = 0; i < max; i++)
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