namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class Launch : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Launch")
        .ManaCost("{1}{U}")
        .Type("Enchantment - Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature has flying.{EOL}When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.")        
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Attach>(e => e.Modifiers(              
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)));
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          })
        .Abilities(
          TriggeredAbility(
            "When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.",
            Trigger<OnZoneChanged>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<ReturnToHand>(e => e.ReturnOwner = true)));
    }
  }
}