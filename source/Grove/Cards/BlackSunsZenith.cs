namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Casting;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class BlackSunsZenith : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Black Sun's Zenith")
        .ManaCost("{B}{B}")
        .Type("Sorcery")
        .Text("Put X -1/-1 counters on each creature. Shuffle Black Sun's Zenith into its owner's library.")
        .FlavorText("'Under the suns, Mirrodin kneels and begs us for perfection.'{EOL}—Geth, Lord of the Vault")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Rule = Rule<Sorcery>(r => r.AfterResolvePutToZone = card => card.ShuffleIntoLibrary());
            p.XCalculator = VariableCost.ReduceCreaturesPwT();
            p.Effect = Effect<ApplyModifiersToPermanents>(e =>
              {
                e.ToughnessReduction = Value.PlusX;
                e.Filter = (effect, card) => card.Is().Creature;
                e.Modifiers(Modifier<AddCounters>(m =>
                  {
                    m.Counter = Counter<PowerToughness>(counter =>
                      {
                        counter.Power = -1;
                        counter.Toughness = -1;
                      });
                    m.Count = Value.PlusX;
                  }));
              });
          });
    }
  }
}