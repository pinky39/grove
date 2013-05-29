namespace Grove.Gameplay.Damage
{
  using System.Linq;
  using Targeting;

  public class PreventLifelossBelowOne : DamagePrevention
  {
    public override int PreventLifeloss(int lifeloss)
    {
      return CalculateLifeloss(lifeloss);
    }

    private int CalculateLifeloss(int lifeloss)
    {
      var controller = Owner.Player();
      var controlsCreature = controller.Battlefield.Creatures.Any();

      if (!controlsCreature)
        return lifeloss;

      var lifeAfterDamage = controller.Life - lifeloss;

      if (lifeAfterDamage < 1)
      {
        return controller.Life - 1;
      }

      return lifeloss;
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      var dealt = amount - CalculateLifeloss(amount);
      return dealt > 0 ? dealt : 0;
    }
  }
}