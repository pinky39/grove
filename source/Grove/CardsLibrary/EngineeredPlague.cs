namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class EngineeredPlague : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Engineered Plague")
        .ManaCost("{2}{B}")
        .Type("Enchantment")
        .Text(
          "As Engineered Plague enters the battlefield, choose a creature type.{EOL}All creatures of the chosen type get -1/-1.")
        .FlavorText("The admixture of bitterwort in the viral brew has produced most favorable results.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new CreaturesOfChosenTypeGetM1M1();
            p.UsesStack = false;
          });
    }
  }
}