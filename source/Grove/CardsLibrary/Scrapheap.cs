namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

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
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, _) => c.Owner == a.OwningCard.Controller && (c.Is().Artifact || c.Is().Enchantment)));

            p.Effect = () => new ChangeLife(amount: 1, forYou: true);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}