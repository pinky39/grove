namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;

  public class FaultLine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fault Line")
        .ManaCost("{R}{R}")
        .Type("Instant")
        .Text("Fault Line deals X damage to each creature without flying and each player.")
        .FlavorText("We live on the serpent's back.{EOL}—Viashino saying")
        .Cast(p =>
          {
            p.XCalculator = ChooseXAi.MaximumAvailableMana();
            p.Timing = Timings.MassRemovalInstantSpeed();
            p.Effect = Effect<Core.Effects.DealDamageToCreaturesAndPlayers>(e =>
              {
                e.AmountPlayer = Value.PlusX;
                e.AmountCreature = Value.PlusX;
                e.FilterCreature = (effect, card) => !card.Has().Flying;
              });
          });
    }
  }
}