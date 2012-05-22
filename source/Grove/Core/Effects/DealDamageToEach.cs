namespace Grove.Core.Effects
{
  public class DealDamageToEach : Effect
  {
    public int Amount { get; set; }
    public bool DealToCreature { get; set; }
    public bool DealToPlayer { get; set; }

    public override void Resolve()
    {
      if (DealToPlayer)
      {
        foreach (var player in Players)
        {
          player.DealDamage(Source.OwningCard, Amount, isCombat: false);
        }
      }

      if (DealToCreature)
      {
        foreach (var player in Players)
        {
          foreach (var creature in player.Battlefield.Creatures)
          {
            creature.DealDamage(Source.OwningCard, Amount, isCombat: false);
          }
        }
      }
    }
  }
}