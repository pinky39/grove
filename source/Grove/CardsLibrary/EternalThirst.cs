namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class EternalThirst : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Eternal Thirst")
        .ManaCost("{1}{B}")
        .Type("Enchantment — Aura")
        .Text("Enchant creature{EOL}Enchanted creature has lifelink and \"Whenever a creature an opponent controls dies, put a +1/+1 counter on this creature.\" {I}(Damage dealt by a creature with lifelink also causes its controller to gain that much life.){/I}")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddSimpleAbility(Static.Lifelink),
            () =>
            {
              var tp = new TriggeredAbility.Parameters()
              {
                Text = "Whenever a creature an opponent controls dies, put a +1/+1 counter on this creature.",
                Effect = () => new ApplyModifiersToSelf(() => new AddCounters(
                  () => new PowerToughness(1, 1), count: 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness),
              };

              tp.Trigger(new OnZoneChanged(
                from: Zone.Battlefield, to: Zone.Graveyard, 
                selector: (c,ctx) => c.Is().Creature && c.Controller == ctx.Opponent));

              return new AddTriggeredAbility(new TriggeredAbility(tp));
            });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCombatEnchantment());
        });
    }
  }
}
