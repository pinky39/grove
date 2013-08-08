namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class FertileGround : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fertile Ground")
        .ManaCost("{1}{G}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant land}{EOL}Whenever enchanted land is tapped for mana, its controller adds one mana of any color to his or her mana pool.")
        .FlavorText("The forest was too lush for the brothers to despoil—almost.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new IncreaseManaOutput(Mana.Any));
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new LandEnchantment(ControlledBy.SpellOwner));
          });
    }
  }
}