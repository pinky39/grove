namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Modifiers;

  public class EleshNornGrandCenobite : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Elesh Norn, Grand Cenobite")
        .ManaCost("{5}{W}{W}")
        .Type("Legendary Creature Praetor")
        .Text("{Vigilance}{EOL}Other creatures you control get +2/+2.{EOL}Creatures your opponents control get -2/-2.")
        .FlavorText(
          "'The Gitaxians whisper among themselves of other worlds. If they exist, we must bring Phyrexia's magnificence to them.'")
        .Power(4)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Category(EffectCategories.ToughnessIncrease | EffectCategories.ToughnessReduction)
        .Abilities(
          StaticAbility.Vigilance,
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddPowerAndToughness>(
                (m, _) =>
                  {
                    m.Power = 2;
                    m.Toughness = 2;
                  });
              e.Filter = (card, source) => card.Controller == source.Controller;
            }),
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddPowerAndToughness>(
                (m, _) =>
                  {
                    m.Power = -2;
                    m.Toughness = -2;
                  });
              e.Filter = (card, source) => card.Controller != source.Controller;
            })
        );
    }
  }
}