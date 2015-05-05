namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class GoblinRabblemaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Goblin Rabblemaster")
          .ManaCost("{2}{R}")
          .Type("Creature — Goblin Warrior")
          .Text("Other Goblin creatures you control attack each turn if able.{EOL}At the beginning of combat on your turn, put a 1/1 red Goblin creature token with haste onto the battlefield.{EOL}Whenever Goblin Rabblemaster attacks, it gets +1/+0 until end of turn for each other attacking Goblin.")
          .Power(2)
          .Toughness(2)
          .ContinuousEffect(p =>
          {
            p.CardFilter = (card, effect) => card.Is("Goblin") && card.Controller == effect.Source.Controller && card != effect.Source;
            p.Modifier = () => new AddStaticAbility(Static.AttacksEachTurnIfAble);
          })
          .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of combat on your turn, put a 1/1 red Goblin creature token with haste onto the battlefield.";
            p.Trigger(new OnStepStart(Step.BeginningOfCombat));
            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Goblin")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Goblin")
                .Text("{Haste}")
                .Colors(CardColor.Red)
                .SimpleAbilities(Static.Haste));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever Goblin Rabblemaster attacks, it gets +1/+0 until end of turn for each other attacking Goblin.";
            p.Trigger(new WhenThisAttacks());
            p.Effect = () => new ApplyModifiersToSelf(
              () => new ModifyPowerToughnessForEachPermanent(
              power: 1,
              toughness: 0,
              filter: (c, m) => c.Is("Goblin") && c.IsAttacker && c != m.OwningCard,
              modifier: () => new IntegerIncrement()
              ));
          });
    }
  }
}
