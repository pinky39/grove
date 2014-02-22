namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Modifiers;

  public class Exploration : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Exploration")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text("You may play an additional land on each of your turns.")
        .FlavorText(
          "The first explorers found Argoth a storehouse of natural wealth—towering forests grown over rich veins of ore.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .StaticAbility(p => p.Modifier(() => new IncreaseLandLimit()));
    }
  }
}