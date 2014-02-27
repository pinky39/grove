namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class ManaLeech : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mana Leech")
        .ManaCost("{2}{B}")
        .Type("Creature Leech")
        .Text(
          "You may choose not to untap Mana Leech during your untap step.{EOL}{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.")
        .Power(1)
        .Toughness(1)
        .MayChooseToUntap()
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.";
            p.Cost = new Tap();

            p.Effect = () => new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);
                  modifier.AddLifetime(new ModifierSourceGetsUntapedLifetime());
                  return modifier;
                }));

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());            
            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
            p.TargetingRule(new EffectTapLand());
          });
    }
  }
}