namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class GoblinFestival : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Festival")
        .ManaCost("{1}{R}")
        .Type("Enchantment")
        .Text("{2}: Goblin Festival deals 1 damage to target creature or player. Flip a coin. If you lose the flip, choose one of your opponents. That player gains control of Goblin Festival.")
        .FlavorText("What are we celebratin' again?")
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Goblin Festival deals 1 damage to target creature or player. Flip a coin. If you lose the flip, choose one of your opponents. That player gains control of Goblin Festival.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            
            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(1), 
              new FlipACoinOpponentGainsOwningCard());

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(p1 => p1.MaxRepetitions));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
            p.RepetitionRule(new RepeatForEachLifepointTargetHasLeft());
          });
    }
  }
}