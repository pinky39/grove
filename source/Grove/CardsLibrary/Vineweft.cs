namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class Vineweft : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vineweft")
        .ManaCost("{G}")
        .Type("Enchantment — Aura")
        .Text("Enchant creature{EOL}Enchanted creature gets +1/+1.{EOL}{4}{G}: Return Vineweft from your graveyard to your hand.")
        .FlavorText("Fortified by the wilds.")
        .Cast(p =>
        {
          p.Effect = () => new Attach(() => new AddPowerAndToughness(1, 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectCombatEnchantment());
          p.TimingRule(new OnFirstMain());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{G}: Return Vineweft from your graveyard to your hand.";

          p.Cost = new PayMana("{4}{G}".Parse());

          p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
          p.ActivationZone = Zone.Graveyard;
          
          p.TimingRule(new OnEndOfOpponentsTurn());
        });
    }
  }
}
