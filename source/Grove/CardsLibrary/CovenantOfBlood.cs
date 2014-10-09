namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class CovenantOfBlood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Covenant of Blood")
        .ManaCost("{6}{B}")
        .Type("Sorcery")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Covenant of Blood deals 4 damage to target creature or player and you gain 4 life.")
        .Convoke()
        .Cast(p =>
        {
          p.Text = "Covenant of Blood deals 4 damage to target creature or player and you gain 4 life.";
          p.Effect = () => new DealDamageToTargets(amount: 4, gainLife: true);

          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

          p.TargetingRule(new EffectDealDamage(4));
        });
    }
  }
}
