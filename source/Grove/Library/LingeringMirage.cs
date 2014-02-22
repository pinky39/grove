namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class LingeringMirage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lingering Mirage")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text("{Enchant land}{EOL}Enchanted land is an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Birds frozen in flight. Sea turned to glass. Tolaria hidden in a mirror.")
        .Cycling("{2}")
        .OverrideScore(new ScoreOverride {Battlefield = 250})
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new ChangeBasicLand("island"));
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());
            p.TargetingRule(new EffectLandEnchantment(ControlledBy.Opponent));
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}