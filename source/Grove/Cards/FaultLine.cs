namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class FaultLine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fault Line")
        .ManaCost("{R}{R}").XCalculator(VariableCost.MaximumAvailableMana())
        .Type("Instant")
        .Text("Fault Line deals X damage to each creature without flying and each player.")
        .FlavorText("We live on the serpent's back.{EOL}—Viashino saying")
        .Timing(Timings.MassRemovalInstantSpeed())
        .Effect<DealDamageToEach>(e =>
          {
            e.AmountPlayer = Value.PlusX;
            e.AmountCreature = Value.PlusX;
            e.FilterCreature = (effect, card) => !card.Has().Flying;
          });        
      }
  }
}