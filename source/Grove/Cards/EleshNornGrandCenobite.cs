namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class EleshNornGrandCenobite : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new PutIntoPlay
              {
                ToughnessReduction = 2,
                Category = EffectCategories.ToughnessIncrease
              };
          })
        .SimpleAbilities(Static.Vigilance)
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(2, 2);
            p.CardFilter = (c, e) => c.Controller == e.Source.Controller && c.Is().Creature && c != e.Source;
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(-2, -2);
            p.CardFilter = (c, e) => c.Controller != e.Source.Controller;
          });
    }
  }
}