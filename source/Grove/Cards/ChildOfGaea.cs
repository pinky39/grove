namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Effects;
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
        .Regenerate(cost: "{1}{G}".Parse(), text: "{1}{G}: Regenerate Child of Gaea.")
        .TriggeredAbility(p =>
          {
            p.Text = "At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new PayManaOrSacrifice("{G}{G}".Parse(), "Pay upkeep? (or sacrifice Child of Gaea)");
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}