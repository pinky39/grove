namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.Modifiers;

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
            p.Effect = () => new CastPermanent
              {
                ToughnessReduction = 2,
              }.SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          })
        .SimpleAbilities(Static.Vigilance)
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(2, 2);
            p.Selector = (c, e) => c.Controller == e.Source.Controller && c.Is().Creature && c != e.Source;
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(-2, -2);
            p.Selector = (c, ctx) => c.Controller != ctx.You;
          });
    }
  }
}