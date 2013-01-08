namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class EleshNornGrandCenobite : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Elesh Norn, Grand Cenobite")
        .ManaCost("{5}{W}{W}")
        .Type("Legendary Creature Praetor")
        .Text("{Vigilance}{EOL}Other creatures you control get +2/+2.{EOL}Creatures your opponents control get -2/-2.")
        .FlavorText(
          "'The Gitaxians whisper among themselves of other worlds. If they exist, we must bring Phyrexia's magnificence to them.'")
        .Power(4)
        .Toughness(7)
        .Cast(p =>
          {
            p.Category = EffectCategories.ToughnessIncrease;
            p.Effect = Effect<PutIntoPlay>(e =>
              {
                e.ToughnessReductionFilter = (self, card) => card.Controller != self.Controller;
                e.ToughnessReduction = 2;
              });
          })
        .Abilities(
          Static.Vigilance,
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddPowerAndToughness>(
                m =>
                  {
                    m.Power = 2;
                    m.Toughness = 2;
                  });
              e.CardFilter =
                (card, effect) =>
                  card.Controller == effect.Source.Controller && card.Is().Creature && card != effect.Source;
            }),
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddPowerAndToughness>(
                m =>
                  {
                    m.Power = -2;
                    m.Toughness = -2;
                  });
              e.CardFilter = (card, effect) => card.Controller != effect.Source.Controller;
            })
        );
    }
  }
}