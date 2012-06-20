namespace Grove.Core.Effects
{
  using System;

  public class DealDamageToEach : Effect
  {
    public Func<Player, int> AmountPlayer;
    public Func<Card, int> AmountCreature;

    public bool DealToCreature { get { return AmountCreature != null; } }
    public bool DealToPlayer { get { return AmountPlayer != null; } }

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