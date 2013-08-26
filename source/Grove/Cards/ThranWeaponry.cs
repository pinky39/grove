namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class ThranWeaponry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Weaponry")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "{Echo} {4}{EOL}You may choose not to untap Thran Weaponry during your untap step.{EOL}{2},{T}: All creatures get +2/+2 for as long as Thran Weaponry remains tapped.")
        .MayChooseToUntap()
        .Echo("{4}")
        .ActivatedAbility(p =>
          {
            p.Text = "{2},{T}: All creatures get +2/+2 for as long as Thran Weaponry remains tapped.";
            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (e, c) => c.Is().Creature,
              modifiers: () =>
                {
                  var modifier = new AddPowerAndToughness(2, 2);
                  modifier.AddLifetime(new ModifierSourceGetsUntapedLifetime());
                  return modifier;
                });
            
            p.TimingRule(new OnYourTurn(Step.DeclareBlockers));
          });
    }
  }
}