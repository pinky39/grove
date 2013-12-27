namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Targeting;
  using Gameplay.Triggers;

  public class SigilOfSleep : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sigil of Sleep")
        .ManaCost("{U}")
        .Type("Enchantment Aura")
        .Text(
          "Whenever enchanted creature deals damage to a player, return target creature that player controls to its owner's hand.")
        .FlavorText("Arrows are only one way to remove an enemy.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever enchanted creature deals damage to a player, return target creature that player controls to its owner's hand.";

            p.Trigger(new OnDamageDealt(
              combatOnly: false,
              useAttachedToAsTriggerSource: true,
              playerFilter: delegate { return true; }));

            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(trg =>
              {
                trg
                  .Is.Card(tp => tp.Target.Card().Is().Creature &&
                    tp.TriggerMessage<DamageHasBeenDealt>().Receiver == tp.Target.Controller())
                  .On.Battlefield();

                p.TargetingRule(new EffectBounce());

                p.TriggerOnlyIfOwningCardIsInPlay = true;
              });
          });
    }
  }
}