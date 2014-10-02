namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class SwordOfFireAndIce : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sword of Fire and Ice")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from red and from blue.{EOL}Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.{EOL}{Equip} {2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.";
            p.Trigger(new OnDamageDealt(
              combatOnly: true,
              useAttachedToAsTriggerSource: true,
              playerFilter: delegate { return true; }));

            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(2),
              new DrawCards(1));

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target creature you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddProtectionFromColors(L(CardColor.Red, CardColor.Blue)))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness, EffectTag.Protection);              
              
            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.TargetingRule(new EffectCombatEquipment());
            p.ActivateAsSorcery = true;
            p.IsEquip = true;
          });
    }
  }
}