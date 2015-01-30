namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class KheruDreadmaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kheru Dreadmaw")
        .ManaCost("{4}{B}")
        .Type("Creature — Zombie Crocodile")
        .Text("{Defender}{EOL}{1}{G}, Sacrifice another creature: You gain life equal to the sacrificed creature's toughness.")
        .FlavorText("Its hunting instincts have long since rotted away. Its hunger, however, remains.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Defender)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{G}, Sacrifice another creature: You gain life equal to the sacrificed creature's toughness.";

          p.Cost = new AggregateCost(
            new PayMana("{1}{G}".Parse(), ManaUsage.Abilities),
            new Sacrifice());

          p.Effect = () => new ChangeLife(amount: P(e => e.Target.Card().Toughness.GetValueOrDefault()), yours: true);

          p.TargetSelector.AddCost(trg =>
          {
            trg
              .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield();

            trg.Message = "Select a creature to sacrifice.";
          });

          p.TargetingRule(new CostSacrificeToGainLife());
        });
    }
  }
}
