namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class AjaniSteadfast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ajani Steadfast")
        .ManaCost("{3}{W}")
        .Type("Planeswalker Ajani")
        .Text(
          "+1: Until end of turn, up to one target creature gets +1/+1 and gains first strike, vigilance, and lifelink.{EOL}-2: Put a +1/+1 counter on each creature you control and a loyalty counter on each other planeswalker you control.{EOL}-7: You get an emblem with 'If a source would deal damage to you or a planeswalker you control, prevent all but 1 of that damage.'")
        .Loyality(4)
        .ActivatedAbility(p =>
          {
            p.Text =
              "+1: Until end of turn, up to one target creature gets +1/+1 and gains first strike, vigilance, and lifelink.";

            p.Cost = new AddCountersCost(CounterType.Loyality, 1);

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true},
              () => new AddStaticAbility(Static.FirstStrike) {UntilEot = true},
              () => new AddStaticAbility(Static.Vigilance) {UntilEot = true},
              () => new AddStaticAbility(Static.Lifelink) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectCombatEnchantment());
            p.TimingRule(new OnFirstMain());
            p.ActivateAsSorcery = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "-2: Put a +1/+1 counter on each creature you control and a loyalty counter on each other planeswalker you control.{EOL}";

            p.Cost = new RemoveCounters(CounterType.Loyality, 2);

            p.Effect = () => new CompoundEffect(
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,
                modifier: () => new AddCounters(() => new PowerToughness(1, 1), 1)),
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is().Planeswalker && ctx.You == c.Controller && ctx.OwningCard != c,
                modifier: () => new AddCounters(() => new SimpleCounter(CounterType.Loyality), 1)));

            p.TimingRule(new OnFirstMain());
            p.ActivateAsSorcery = true;
          })
        .ActivatedAbility(p =>
          {                        
            p.Text =
              "-7: You get an emblem with 'If a source would deal damage to you or a planeswalker you control, prevent all but 1 of that damage.'";

            p.Cost = new RemoveCounters(CounterType.Loyality, 7);

            p.Effect = () => new CreateEmblem(
              text: "If a source would deal damage to you or a planeswalker you control, prevent all but 1 of that damage.",
              score: 400,
              controller: P(e => e.Controller),
              modifiers: () => new AddDamagePrevention(new PreventDamageToPermanentsOrPlayers(
                (player, ctx) => ctx.You == player, 
                (c, ctx) => c.Controller == ctx.You && c.Is().Planeswalker,
                (target, ctx) => ctx.AmountDealt - 1)));

            p.TimingRule(new OnFirstMain());
            p.ActivateAsSorcery = true;            
          });
    }
  }
}