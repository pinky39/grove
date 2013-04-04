namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Sicken : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sicken")
        .ManaCost("{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature gets -1/-1.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Urza dared to attack Phyrexia. Slowly, it retaliated.")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(-1, -1)) {ToughnessReduction = 1};
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new ReduceToughness(1));
          });
    }
  }
}