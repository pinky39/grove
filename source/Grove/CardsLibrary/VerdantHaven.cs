namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class VerdantHaven : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Verdant Haven")
        .ManaCost("{2}{G}")
        .Type("Enchantment — Aura")
        .Text("Enchant land{EOL}When Verdant Haven enters the battlefield, you gain 2 life.{EOL}Whenever enchanted land is tapped for mana, its controller adds one mana of any color to his or her mana pool {I}(in addition to the mana the land produces).{/I}")
        .Cast(p =>
        {
          p.Effect = () => new Attach(() => new IncreaseManaOutput(Mana.Any));
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectLandEnchantment(ControlledBy.SpellOwner));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Verdant Haven enters the battlefield, you gain 2 life.";

          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new ChangeLife(2, forYou: true);
        });
    }
  }
}
