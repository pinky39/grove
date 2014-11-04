namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ArrowStorm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Arrow Storm")
        .ManaCost("{3}{R}{R}")
        .Type("Sorcery")
        .Text("Arrow Storm deals 4 damage to target creature or player.{EOL}{I}Raid{/I} — If you attacked with a creature this turn, instead Arrow Storm deals 5 damage to that creature or player and the damage can't be prevented.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(P((e, g) => g.Turn.Events.HasActivePlayerAttackedThisTurn ? 5 : 4),
            canBePrevented: P((e, g) => !g.Turn.Events.HasActivePlayerAttackedThisTurn));
          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(tp => tp.Controller.HasAttackedThisTurn ? 5 : 4));
          p.TimingRule(new OnSecondMain());
        });
    }
  }
}
