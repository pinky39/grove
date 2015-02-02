namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Vaultbreaker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vaultbreaker")
        .ManaCost("{3}{R}")
        .Type("Creature — Orc Rogue")
        .Text("Whenever Vaultbreaker attacks, you may discard a card. If you do, draw a card.{EOL}Dash {2}{R}{I}(You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.){/I}")
        .Power(4)
        .Toughness(2)
        .Cast(p => { p.Effect = () => new CastPermanent(); })
        .Cast(p =>
        {
          p.Cost = new PayMana("{2}{R}".Parse(), ManaUsage.Spells);
          p.Text = "{{2}}{{R}}: Dash";
          p.Effect = () => new CompoundEffect(
            new CastPermanent(),
            new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Dash) { UntilEot = true },
              () => new AddStaticAbility(Static.Haste) { UntilEot = true }));
          p.TimingRule(new OnFirstMain());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.";
          p.Trigger(new OnStepStart(step: Step.EndOfTurn)
          {
            Condition = (t, g) => t.OwningCard.Has().Dash
          });

          p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Vaultbreaker attacks, you may discard a card. If you do, draw a card.";
          p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat && dmg.IsDealtByOwningCard && dmg.IsDealtToPlayer));

          p.Effect = () => new DiscardCardToDrawCard();
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
