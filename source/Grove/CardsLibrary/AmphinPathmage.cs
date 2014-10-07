namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class AmphinPathmage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Amphin Pathmage")
        .ManaCost("{3}{U}")
        .Type("Creature - Salamander Wizard")
        .Text("{2}{U}: Target creature can't be blocked this turn.")
        .FlavorText("\"There are those who do not believe in the existence of the amphin. This seems somehow to be of their own design.\"{EOL}—Gor Muldrak, Cryptohistories ")
        .Power(3)
        .Toughness(2)
        .ActivatedAbility(p =>
        {
          p.Text = "{2}{U}: Target creature can't be blocked this turn.";
          p.Cost = new PayMana("{2}{U}".Parse(), ManaUsage.Abilities);

          p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Unblockable) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TimingRule(new BeforeYouDeclareAttackers());
          p.TargetingRule(new EffectBigWithoutEvasions());
        });
    }
  }
}
