namespace Grove.Core.Effects
{
  using System;
  using Modifiers;

  public class DealDamageToEach : Effect
  {
    public Value AmountCreature = 0;
    public Value AmountPlayer = 0;

    public Func<DealDamageToEach, Card, bool> FilterCreature = delegate { return true; };
    public Func<DealDamageToEach, Player, bool> FilterPlayer = delegate { return true; };

    private bool DealToCreature(Card creature)
    {
      return AmountCreature != null && FilterCreature(this, creature);
    }

    private bool DealToPlayer(Player player)
    {
      return AmountPlayer != null && FilterPlayer(this, player);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return DealToPlayer(player) ? AmountPlayer.GetValue(X) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return DealToCreature(creature) ? AmountCreature.GetValue(X) : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var player in Core.Players)
      {
        if (DealToPlayer(player))
        {
          var damage = new Damage(
            source: Source.OwningCard,
            amount: AmountPlayer.GetValue(X),
            isCombat: false,
            changeTracker: Game.ChangeTracker
            );

          player.DealDamage(damage);
        }
      }

      foreach (var player in Core.Players)
      {
        foreach (var creature in player.Battlefield.Creatures)
        {
          if (DealToCreature(creature))
          {
            var damage = new Damage(
              source: Source.OwningCard,
              amount: AmountCreature.GetValue(X),
              isCombat: false,
              changeTracker: Game.ChangeTracker
              );

            creature.DealDamage(damage);
          }
        }
      }
    }
  }
}