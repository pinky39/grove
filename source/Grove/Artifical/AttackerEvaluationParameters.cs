namespace Grove.Artifical
{
  using System.Collections.Generic;
  using Gameplay;

  public class AttackerEvaluationParameters
  {
    public List<Blocker> Blockers = new List<Blocker>();

    public AttackerEvaluationParameters(Card attacker, Card blocker, int attackerPowerIncrease,
      int attackerToughnessIncrease, int blockerPowerIncrease, int blockerToughnessIncrease)
    {
      Attacker = attacker;
      AttackerToughnessIncrease = attackerToughnessIncrease;
      AttackerPowerIncrease = attackerPowerIncrease;

      AddBlocker(blocker, blockerPowerIncrease, blockerToughnessIncrease);
    }

    public AttackerEvaluationParameters(Card attacker, int attackerPowerIncrease, int attackerToughnessIncrease)
    {
      Attacker = attacker;
      AttackerPowerIncrease = attackerPowerIncrease;
      AttackerToughnessIncrease = attackerToughnessIncrease;
    }

    public AttackerEvaluationParameters(Card attacker, IEnumerable<Card> blockers)
    {
      Attacker = attacker;

      foreach (var blocker in blockers)
      {
        AddBlocker(blocker);
      }
    }

    public Card Attacker { get; private set; }
    public int AttackerToughnessIncrease { get; set; }
    public int AttackerPowerIncrease { get; set; }

    public void AddBlocker(Card card, int powerIncrease = 0, int toughnessIncrease = 0)
    {
      Blockers.Add(new Blocker {Card = card, PowerIncrease = powerIncrease, ToughnessIncrease = toughnessIncrease});
    }

    public class Blocker
    {
      public Card Card;
      public int PowerIncrease;
      public int ToughnessIncrease;
    }
  }
}