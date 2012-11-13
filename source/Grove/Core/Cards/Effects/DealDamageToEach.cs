namespace Grove.Core.Cards.Effects
{
  using System;

  public class DealDamageToEach : Effect
  {
    public int? AmountCreature;
    public int? AmountPlayer;

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
      return DealToPlayer(player) ? AmountPlayer.GetValueOrDefault() : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return DealToCreature(creature) ? AmountCreature.GetValueOrDefault() : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var player in Players)
      {
        if (DealToPlayer(player))
        {
          var damage = new Damage(
            source: Source.OwningCard,
            amount: AmountPlayer.Value,
            isCombat: false,
            changeTracker: Game.ChangeTracker
            );

          player.DealDamage(damage);
        }
      }

      foreach (var player in Players)
      {
        foreach (var creature in player.Battlefield.Creatures)
        {
          if (DealToCreature(creature))
          {
            var damage = new Damage(
              source: Source.OwningCard,
              amount: AmountCreature.Value,
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