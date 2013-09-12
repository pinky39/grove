namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class KingCrab : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("King Crab")
        .ManaCost("{4}{U}{U}")
        .Type("Creature Crab")
        .Text("{1}{U},{T}: Put target green creature on top of its owner's library.")
        .FlavorText("I'm allergic to shellfish—not only did it kill my entire village, it also gave me this nasty rash.")
        .Power(4)
        .Toughness(5)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{U},{T}: Put target green creature on top of its owner's library.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{U}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new PutTargetOnTopOfLibrary();

            p.TargetSelector.AddEffect(trg =>
              trg.Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Green)).On.Battlefield());

            p.TargetingRule(new EffectPutOnTopOfLibrary());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly));
          });
    }
  }
}