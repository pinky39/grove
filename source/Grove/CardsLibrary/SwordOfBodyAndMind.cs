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

  public class SwordOfBodyAndMind : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sword of Body and Mind")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from green and from blue.{EOL}Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.{EOL}{Equip} {2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat &&
                dmg.IsDealtByEnchantedCreature &&
                dmg.IsDealtToPlayer));              

            p.Effect = () => new CompoundEffect(
              new PlayerPutsTopCardsFromLibraryToGraveyard(P(e => e.Controller.Opponent), count: 10),
              new CreateTokens(
                count: 1,
                token: Card
                  .Named("Wolf")
                  .FlavorText(
                    "No matter where we cat warriors go in the world, those stupid slobberers find us.")
                  .Power(2)
                  .Toughness(2)
                  .Type("Token Creature - Wolf")
                  .Colors(CardColor.Green)));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target creature you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddProtectionFromColors(L(CardColor.Green, CardColor.Blue)))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness, EffectTag.Protection);              

            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.ActivateAsSorcery = true;
            p.IsEquip = true;
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.TargetingRule(new EffectCombatEquipment());
          });
    }
  }
}