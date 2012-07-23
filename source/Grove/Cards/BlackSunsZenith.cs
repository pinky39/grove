namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Counters;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Zones;

  public class BlackSunsZenith : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Black Sun's Zenith")
        .ManaCost("{B}{B}").XCalculator(VariableCost.ReduceCreaturesPwT())
        .Type("Sorcery")
        .Text("Put X -1/-1 counters on each creature. Shuffle Black Sun's Zenith into its owner's library.")
        .AfterResolvePutToZone(Zone.Library)        
        .Timing(Timings.FirstMain())
        .FlavorText("'Under the suns, Mirrodin kneels and begs us for perfection.'{EOL}—Geth, Lord of the Vault")
        .Effect<ApplyModifiersToCreatures>(p =>
          {
            p.Effect.ToughnessReduction = Value.PlusX;
            p.Effect.Modifiers(p.Builder.Modifier<AddCounters>((m, c0) =>
              {
                m.Counter = c0.Counter<PowerToughness>((counter, _) =>
                  {
                    counter.Power = -1;
                    counter.Toughness = -1;
                  });
                m.Count = Value.PlusX;
              }));
          });
    }
  }
}