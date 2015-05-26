namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class SoulOfShandalar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Shandalar")
        .ManaCost("{4}{R}{R}")
        .Type("Creature — Avatar")
        .Text(
          "{First strike}{EOL}{3}{R}{R}: Soul of Shandalar deals 3 damage to target player and 3 damage to up to one target creature that player controls.{EOL}{3}{R}{R}, Exile Soul of Shandalar from your graveyard: Soul of Shandalar deals 3 damage to target player and 3 damage to up to one target creature that player controls.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.FirstStrike)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{3}{R}{R}: Soul of Shandalar deals 3 damage to target player and 3 damage to up to one target creature that player controls.";
            p.Cost = new PayMana("{3}{R}{R}".Parse(), supportsRepetitions: true);

            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector
              .AddEffect(
                trg => trg.Is.Player(),
                trg => { trg.Message = "Select a player."; })
              .AddEffect(
                trg => trg.Is.Creature().On.Battlefield(),
                trg =>
                  {
                    trg.MinCount = 0;
                    trg.Message = "Select a creature that player controls.";
                  })
              .ValidateTargetDependencies = vp =>
                {
                  if (vp.Effect.Count == 1)
                    return true;
                  
                  return vp.Effect[0].Player() == vp.Effect[1].Card().Controller;
                };

            p.TargetingRule(new EffectDealDamage(3));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
            p.RepetitionRule(new RepeatMaxTimes());
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{3}{R}{R}, Exile Soul of Shandalar from your graveyard: Soul of Shandalar deals 3 damage to target player and 3 damage to up to one target creature that player controls.";
            p.Cost = new AggregateCost(
              new PayMana("{3}{R}{R}".Parse()),
              new Exile(fromGraveyard: true));

            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector
              .AddEffect(
                trg => trg.Is.Player(),
                trg => { trg.Message = "Select a player."; })
              .AddEffect(
                trg => trg.Is.Creature().On.Battlefield(),
                trg =>
                  {
                    trg.MinCount = 0;                    
                    trg.Message = "Select a creature that player controls.";
                  })
              .ValidateTargetDependencies = vp =>
                {
                  if (vp.Effect.Count == 1)
                    return true;

                  return vp.Effect[0].Player() == vp.Effect[1].Card().Controller;
                };

            p.TargetingRule(new EffectDealDamage(3));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}