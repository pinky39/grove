namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.States;

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
        .StaticAbilities(Static.Trample)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}: Regenerate Child of Gaea.";
            p.Cost = new PayMana("{1}{G}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Ai.TimingRules.Regenerate());
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