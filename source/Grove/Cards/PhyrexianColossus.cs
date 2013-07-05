namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

  public class PhyrexianColossus : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new OwningCardHas(c => c.IsTapped));            
            p.TimingRule(new Steps(activeTurn: true, passiveTurn: true, steps: Step.BeginningOfCombat));
          });
    }
  }
}