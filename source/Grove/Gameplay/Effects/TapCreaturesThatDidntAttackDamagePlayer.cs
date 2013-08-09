namespace Grove.Gameplay.Effects
{
  using System.Linq;

  public class TapCreaturesThatDidntAttackDamagePlayer : Effect
  {
    protected override void ResolveEffect()
    {
      var player = Players.Active;
      var damageAmount = 0;

      foreach (var creature in player.Battlefield.Creatures.Where(x => !x.IsTapped))
      {
        if (Turn.Events.HasAttacked(creature) == false)
        {
          creature.Tap();
          damageAmount++;
        }
      }

      if (damageAmount > 0)
      {
        Source.OwningCard.DealDamageTo(
          damageAmount, player, isCombat: false);
      }
    }
  }
}