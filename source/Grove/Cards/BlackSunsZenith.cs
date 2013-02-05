namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.CostRules;
  using Core.Ai.TimingRules;
  using Core.Casting;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class BlackSunsZenith : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Black Sun's Zenith")
        .ManaCost("{B}{B}").HasXInCost()
        .Type("Sorcery")
        .Text("Put X -1/-1 counters on each creature. Shuffle Black Sun's Zenith into its owner's library.")
        .FlavorText("'Under the suns, Mirrodin kneels and begs us for perfection.'{EOL}—Geth, Lord of the Vault")
        .Cast(p =>
          {
            p.Rule = new Sorcery(c => c.ShuffleIntoLibrary());
            p.Effect = () => new ApplyModifiersToPermanents(
              filter: (effect, card) => card.Is().Creature,
              modifiers: () => new AddCounters(() => new PowerToughness(-1, -1), Value.PlusX))
              {ToughnessReduction = Value.PlusX};

            p.TimingRule(new FirstMain());
            p.CostRule(new ReduceToughnessOfEachCreature());
          });
    }
  }
}