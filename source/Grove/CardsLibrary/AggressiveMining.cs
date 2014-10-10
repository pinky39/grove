namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class AggressiveMining : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aggressive Mining")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text("You can't play lands.{EOL}Sacrifice a land: Draw two cards. Activate this ability only once each turn.")
        .StaticAbility(p => p.Modifier(() => new IncreaseLandLimit(amount: -1000))) // Just to avoid effects of spells increase land limit
        .ActivatedAbility(p =>
        {
          p.Text = "Sacrifice a land: Draw two cards. Activate this ability only once each turn.";

          p.Cost = new Sacrifice();

          p.TargetSelector.AddCost(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());

          p.Effect = () => new DrawCards(2);

          p.TargetingRule(new CostSacrificeToDrawCards());

          p.ActivateOnlyOnceEachTurn = true;
        });
    }
  }
}
