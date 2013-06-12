namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class ChildOfGaea : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Child of Gaea")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature Elemental")
        .Text(
          "{Trample}{EOL}At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.{EOL}{1}{G}: Regenerate Child of Gaea.")
        .Power(7)
        .Toughness(7)
        .SimpleAbilities(Static.Trample)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}: Regenerate Child of Gaea.";
            p.Cost = new PayMana("{1}{G}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Artifical.TimingRules.Regenerate());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new PayManaOrSacrifice("{G}{G}".Parse(), "Pay upkeep?");
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}