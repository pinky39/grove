namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;

  public class HuntTheWeak : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hunt the Weak")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text(
          "Put a +1/+1 counter on target creature you control. Then that creature fights target creature you don't control. {I}(Each deals damage equal to its power to the other.){/I}")
        .FlavorText("He who hesitates is lunch.")
        .Cast(p =>
          {
            p.Text =
              "Put a +1/+1 counter on target creature you control. Then that creature fights target creature you don't control.";
            p.Effect = () => new PutCounterOnYoursAndFightWithOpponentsCreature(
              () => new PowerToughness(1, 1), 1).SetTags(EffectTag.Bounce);

            p.TargetSelector.AddEffect(
              trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield(),
              trg => { trg.Message = "Select target creature you control."; });

            p.TargetSelector.AddEffect(
              trg => trg.Is.Creature(ControlledBy.Opponent).On.Battlefield(),
              trg => { trg.Message = "Select target creature your opponent controls."; });

            p.TargetingRule(new EffectCreaturesDealsDamageEqualToPowerToEachOther(1, 1));
          });
    }
  }
}