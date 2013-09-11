namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class SwordOfFeastAndFamine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sword of Feast and Famine")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from black and from green.{EOL}Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.{EOL}{Equip} {2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.";

            p.Trigger(new OnDamageDealt(
              combatOnly: true,
              useAttachedToAsTriggerSource: true,
              playerFilter: delegate { return true; }));

            p.Effect = () => new CompoundEffect(
              new OpponentDiscardsCards(selectedCount: 1),
              new UntapAllLands());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target creature you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddProtectionFromColors(L(CardColor.Black, CardColor.Green)))
              .Tags(EffectTag.IncreasePower, EffectTag.IncreaseToughness, EffectTag.Protection);                            

            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.TargetingRule(new EffectCombatEquipment());
            p.ActivateAsSorcery = true;
          });
    }
  }
}