namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MarduCharm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Charm")
        .ManaCost("{R}{W}{B}")
        .Type("Instant")
        .Text("Choose one —{EOL}• Mardu Charm deals 4 damage to target creature.{EOL}• Put two 1/1 white Warrior creature tokens onto the battlefield. They gain first strike until end of turn.{EOL}• Target opponent reveals his or her hand. You choose a noncreature, nonland card from it. That player discards that card.")
        .Cast(p =>
        {
          p.Text = "{{R}}{{W}}{{B}}: Mardu Charm deals 4 damage to target creature.";
          p.Effect = () => new DealDamageToTargets(4);
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(4));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        })
        .Cast(p =>
        {
          p.Text = "{{R}}{{W}}{{B}}: Put two 1/1 white Warrior creature tokens onto the battlefield. They gain first strike until end of turn.";
          p.Effect = () => new CreateTokens(
            count: 2,
            token: Card
              .Named("Warrior")
              .Power(1)
              .Toughness(1)
              .Type("Token Creature - Warrior")
              .Colors(CardColor.White)
              .TriggeredAbility(tp =>
              {
                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                tp.Effect = () => new ApplyModifiersToSelf(() => new AddSimpleAbility(Static.FirstStrike){UntilEot = true});
                tp.UsesStack = false;
              }));
          
          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn()));
        })
        .Cast(p =>
        {
          p.Text = "{{R}}{{W}}{{B}}: Target opponent reveals his or her hand. You choose a noncreature, nonland card from it. That player discards that card.";
          p.Effect = () => new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => !card.Is().Creature && !card.Is().Land);

          p.TimingRule(new OnFirstMain());
          p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
        });
    }
  }
}
