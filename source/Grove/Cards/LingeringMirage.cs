namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class LingeringMirage : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TargetingRule(new LandEnchantment(ControlledBy.Opponent));
            p.TimingRule(new FirstMain());
          });
    }
  }
}