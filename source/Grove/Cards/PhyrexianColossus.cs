namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

  public class PhyrexianColossus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Colossus")
        .ManaCost("{7}")
        .Type("Artifact Creature Golem")
        .Text(
          "Phyrexian Colossus doesn't untap during your untap step.{EOL}Pay 8 life: Untap Phyrexian Colossus.{EOL}Phyrexian Colossus can't be blocked except by three or more creatures.")
        .Power(8)
        .Toughness(8)
        .IsUnblockableIfNotBlockedByAtLeast(3)
        .SimpleAbilities(Static.DoesNotUntap)
        .ActivatedAbility(p =>
          {
            p.Text = "Pay 8 life: Untap Phyrexian Colossus.";
            p.Cost = new PayLife(8);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));            
            p.TimingRule(new OnStep(Step.BeginningOfCombat));
          });
    }
  }
}