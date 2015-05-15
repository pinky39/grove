namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class ForgeDevil : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Forge Devil")
        .ManaCost("{R}")
        .Type("Creature — Devil")
        .Text("When Forge Devil enters the battlefield, it deals 1 damage to target creature and 1 damage to you.")
        .FlavorText("A bit of pain never hurts.")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Forge Devil enters the battlefield, it deals 1 damage to target creature and 1 damage to you.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(1),
              new DealDamageToPlayer(1, P(e => e.Controller)));
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Message = "Select a creature.";
                trg.MinCount = 1;
                trg.MaxCount = 1;
                trg.Is.Creature().On.Battlefield();
              });
            p.TargetingRule(new EffectDealDamage(1));
          });
    }
  }
}