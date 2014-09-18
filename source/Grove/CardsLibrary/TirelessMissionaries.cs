namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class TirelessMissionaries : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Tireless Missionaries")
                .ManaCost("{4}{W}")
                .Type("Creature - Human Cleric")
                .Text("When Tireless Missionaries enters the battlefield, you gain 3 life.")
                .FlavorText("If they succeed in their holy work, their order will vanish into welcome obscurity, for there will be no more souls to redeem.")
                .Power(2)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Tireless Missionaries enters the battlefield, you gain 3 life.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new YouGainLife(3);
                });
        }
    }
}
