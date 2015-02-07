namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class Pyrotechnics : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pyrotechnics")
        .ManaCost("{4}{R}")
        .Type("Sorcery")
        .Text("Pyrotechnics deals 4 damage divided as you choose among any number of target creatures and/or players.")
        .FlavorText("\"Take inspiration from your enemies, and make their strengths your own.\"{EOL}—Alesha, Who Smiles at Death")
        .Cast(p =>
        {
          p.Effect = () => new DistributeDamageToTargets();
          p.DistributeAmount = 4;
          p.TargetSelector.AddEffect(trg =>
          {
            trg.Is.CreatureOrPlayer().On.Battlefield();
            trg.MaxCount = 4;
          });
          p.TargetingRule(new EffectDealDamage());
        });
    }
  }
}
