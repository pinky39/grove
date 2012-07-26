namespace Grove.Core.Details.Cards.Effects
{
  public class DealDamageToEach : Effect
  {
    public int? AmountCreature;
    public int? AmountPlayer;

    private bool DealToCreature { get { return AmountCreature != null; } }
    private bool DealToPlayer { get { return AmountPlayer != null; } }

    public override int CalculatePlayerDamage(Player player)
    {
      return DealToPlayer ? AmountPlayer.Value : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return DealToCreature ? AmountCreature.Value : 0;
    }

    protected override void ResolveEffect()
    {
      if (DealToPlayer)
      {
        foreach (var player in Players)
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

      if (DealToCreature)
      {
        foreach (var player in Players)
        {
          foreach (var creature in player.Battlefield.Creatures)
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