namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class Endoskeleton : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Endoskeleton")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "You may choose not to untap Endoskeleton during your untap step.{EOL}{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.")
        .MayChooseNotToUntapDuringUntap()
        .ActivatedAbility(p =>
          {
            p.Text = "{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.";
            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap());
            
            p.Effect = () => new ApplyModifiersToTargets(() =>
              {
                var modifier = new AddPowerAndToughness(0, 3);
                modifier.AddLifetime(new PermanentGetsUntapedLifetime(l => l.Modifier.Source));
                return modifier;
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new IncreasePowerOrToughness(0, 3, untilEot: false));
          });
    }
  }
}