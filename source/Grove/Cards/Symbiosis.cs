namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class Symbiosis : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Symbiosis")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Two target creatures each get +2/+2 until end of turn.")
        .FlavorText(
          "Although the elves of Argoth always considered them a nuisance, the pixies made fine allies during the war against the machines.")
        .Cast(p =>
          {
            p.Effect =
              () =>
                new ApplyModifiersToTargets(() => new AddPowerAndToughness(2, 2) {UntilEot = true})
                  {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new IncreasePowerOrToughness(2, 2));
          });
    }
  }
}