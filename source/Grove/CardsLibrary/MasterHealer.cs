namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class MasterHealer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Master Healer")
        .ManaCost("{4}{W}")
        .Type("Creature Human Cleric")
        .Text("{T}: Prevent the next 4 damage that would be dealt to target creature or player this turn.")
        .FlavorText("Behind his eyes is the pain of every soldier his hands have healed.")
        .Power(1)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Prevent the next 4 damage that would be dealt to target creature or player this turn.";
            p.Cost = new Tap();
            p.Effect = () => new PreventNextDamageToTargets(4);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectPreventNextDamageToTargets(4));
          });
    }
  }
}