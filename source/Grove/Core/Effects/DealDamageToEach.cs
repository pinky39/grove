namespace Grove.Core.Effects
{
  using System;
  using Ai;

  public class DealDamageToEach : Effect, IDamageDealing
  {
    public Func<Card, int> AmountCreature;
    public Func<Player, int> AmountPlayer;

    public bool DealToCreature { get { return AmountCreature != null; } }
    public bool DealToPlayer { get { return AmountPlayer != null; } }

    public int PlayerDamage(Player player)
    {
      return DealToPlayer ? AmountPlayer(player) : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return DealToCreature ? AmountCreature(creature) : 0;
    }

    protected override void ResolveEffect()
    {
      if (DealToPlayer)
      {
        foreach (var player in Players)
        {
          player.DealDamage(Source.OwningCard, AmountPlayer(player), isCombat: false);
        }
      }

      if (DealToCreature)
      {
        foreach (var player in Players)
        {
          foreach (var creature in player.Battlefield.Creatures)
          {
            creature.DealDamage(Source.OwningCard, AmountCreature(creature), isCombat: false);
          }
        }
      }
    }
  }
}