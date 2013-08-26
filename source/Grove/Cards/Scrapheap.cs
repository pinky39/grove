namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Scrapheap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scrapheap")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever an artifact or enchantment is put into your graveyard from the battlefield, you gain 1 life.")
        .FlavorText("Junk heaps have rats, but scrapheaps have goblins.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an artifact or enchantment is put into your graveyard from the battlefield, you gain 1 life.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, _) => c.Owner == a.OwningCard.Controller && (c.Is().Artifact || c.Is().Enchantment)));

            p.Effect = () => new ControllerGainsLife(1);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}