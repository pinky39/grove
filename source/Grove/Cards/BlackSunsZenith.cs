namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.CostRules;
  using Ai.TimingRules;
  using Gameplay.Card.CastingRules;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class BlackSunsZenith : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Black Sun's Zenith")
        .ManaCost("{B}{B}").HasXInCost()
        .Type("Sorcery")
        .Text("Put X -1/-1 counters on each creature. Shuffle Black Sun's Zenith into its owner's library.")
        .FlavorText("Under the suns, Mirrodin kneels and begs us for perfection.")
        .Cast(p =>
          {
            p.Rule = new Sorcery(c => c.ShuffleIntoLibrary());
            p.Effect = () => new ApplyModifiersToPermanents(
              permanentFilter: (effect, card) => card.Is().Creature,
              modifiers: () => new AddCounters(() => new PowerToughness(-1, -1), Value.PlusX))
              {ToughnessReduction = Value.PlusX};

            p.TimingRule(new FirstMain());
            p.CostRule(new ReduceToughnessOfEachCreature());
          });
    }
  }
}