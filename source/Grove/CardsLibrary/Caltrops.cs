namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Triggers;

  public class Caltrops : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Caltrops")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever a creature attacks, Caltrops deals 1 damage to it.")
        .FlavorText("Toward the end, Gatha struggled to keep his experiments in and his patrons out.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature attacks, Caltrops deals 1 damage to it.";
            p.Trigger(new OnAttack(triggerForCreature: (c, t) => true, onlyWhenDeclared: false));
            p.Effect = () => new DealDamageToCreature(1, P(e => e.TriggerMessage<AttackerJoinedCombatEvent>().Attacker.Card));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}