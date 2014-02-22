namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.CostRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class BlackSunsZenith : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              selector: (effect, card) => card.Is().Creature,
              modifiers: () => new AddCounters(() => new PowerToughness(-1, -1), Value.PlusX))
              {ToughnessReduction = Value.PlusX};

            p.TimingRule(new OnFirstMain());
            p.CostRule(new XIsOptimalDamage());
          });
    }
  }
}