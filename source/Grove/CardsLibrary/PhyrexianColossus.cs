namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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