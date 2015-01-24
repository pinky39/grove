namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class AnafenzaTheForemost : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Anafenza, the Foremost")
        .ManaCost("{W}{B}{G}")
        .Type("Legendary Creature — Human Soldier")
        .Text("Whenever Anafenza, the Foremost attacks, put a +1/+1 counter on another target tapped creature you control.{EOL}If a creature card would be put into an opponent's graveyard from anywhere, exile it instead.")
        .FlavorText("Rarely at rest on the Amber Throne, Anafenza always leads the Abzan Houses to battle.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Anafenza, the Foremost attacks, put a +1/+1 counter on another target tapped creature you control.";
          p.Trigger(new WhenThisAttacks());
          p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(() => new PowerToughness(1, 1), count: 1));

          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 0;
            trg.MaxCount = 1;
            trg.Message = "Select another target tapped creature you control";
            trg.Is.Card(c => c.Is().Creature && c.IsTapped, controlledBy: ControlledBy.SpellOwner, canTargetSelf: false).On.Battlefield();
          });

          p.TargetingRule(new EffectCombatEnchantment());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "If a creature card would be put into an opponent's graveyard from anywhere, exile it instead.";
          p.Trigger(new OnZoneChanged(
            to: Zone.Graveyard,
            filter: (card, ability, _) => ability.OwningCard.Controller != card.Controller));
          p.Effect = () => new ExileCard(P(e => e.TriggerMessage<ZoneChangedEvent>().Card), Zone.Graveyard);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
