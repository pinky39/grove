namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class DerangedHermit : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Deranged Hermit")
        .ManaCost("{3}{G}{G}")
        .Type("Creature Elf")
        .Text(
          "{Echo} {3}{G}{G}{EOL}When Deranged Hermit enters the battlefield, put four 1/1 green Squirrel creature tokens onto the battlefield.{EOL}Squirrel creatures get +1/+1.")
        .Power(1)
        .Toughness(1)
        .Echo("{3}{G}{G}")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Deranged Hermit enters the battlefield, put four 1/1 green Squirrel creature tokens onto the battlefield.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CreateTokens(
              count: 4,
              token: Card
                .Named("Squirrel")
                .FlavorText(
                  "And the ignorant shall fall to the squirrels.")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Squirrel")
                .Colors(CardColor.Green));
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(1, 1);
            p.Selector = (c, ctx) => c.Is().Creature && c.Is("squirrel");
          });
    }
  }
}