namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class NetcasterSpider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Netcaster Spider")
        .ManaCost("{2}{G}")
        .Type("Creature — Spider")
        .Text("{Reach} {I}(This creature can block creatures with flying.){/I}{EOL}Whenever Netcaster Spider blocks a creature with flying, Netcaster Spider gets +2/+0 until end of turn.")
        .FlavorText("It is an expert at culling individuals who stray too far from the herd.")
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(Static.Reach)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Netcaster Spider blocks a creature with flying, Netcaster Spider gets +2/+0 until end of turn.";

          p.Trigger(new OnBlock(blocks: true, attackerFilter: card => card.Has().Flying));

          p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 0){UntilEot = true});
        });
    }
  }
}
