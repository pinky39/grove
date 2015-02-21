namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class ChildOfGaea : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new PayManaThen("{G}{G}".Parse(),
              effect: new SacrificeOwner(),
              parameters: new PayThen.Parameters()
              {
                ExecuteIfPaid = false,
                Message = "Pay upkeep? (or sacrifice Child of Gaea)",
              });
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}