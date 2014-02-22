namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;
  using Grove.Infrastructure;

  public class ThranQuarry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Quarry")
        .Type("Land")
        .Text(
          "At the beginning of the end step, if you control no creatures, sacrifice Thran Quarry.{EOL}{T}: Add one mana of any color to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add one mana of any color to your mana pool.";
            p.ManaAmount(Mana.Any);
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, if you control no creatures, sacrifice Thran Quarry.";
            p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) => t.Ability.OwningCard.Controller.Battlefield.None(x => x.Is().Creature)
              });

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}