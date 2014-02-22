namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class WeatherseedElf : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Weatherseed Elf")
        .Type("Creature Elf")
        .ManaCost("{G}")
        .Text("{T}: Target creature gains forestwalk until end of turn.")
        .FlavorText(
          "My grandmother once told me the future of our world was inside the Weatherseed. When I touched it, I knew she was right.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Target creature gains forestwalk until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Forestwalk) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new BeforeYouDeclareAttackers());
            p.TargetingRule(new EffectBigWithoutEvasions());
          });
    }
  }
}