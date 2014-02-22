namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Modifiers;

  public class VernalBloom : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vernal Bloom")
        .ManaCost("{3}{G}")
        .Type("Enchantment")
        .Text("Whenever a forest is tapped for mana, it produces an additional {G}.")
        .FlavorText("Many cultures have legends of a lush, hidden paradise. The elves of Argoth had no need of such stories.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new IncreaseManaOutput(Mana.Green);
            p.CardFilter = (card, effect) => card.Is("forest");            
          });
    }
  }
}