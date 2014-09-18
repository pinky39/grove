namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class TurnToFrog : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Turn to Frog")
              .ManaCost("{1}{U}")
              .Type("Instant")
              .Text("Until end of turn, target creature loses all abilities and becomes a blue Frog with base power and toughness 1/1.")
              .FlavorText("\"Ribbit.\"")
              .Cast(p =>
              {
                  p.Effect = () => new ApplyModifiersToTargets(new CardModifierFactory[]
                  {
                      () =>
                        {
                            var pr = new ContinuousEffectParameters
                            {
                                Modifier = () =>
                                {
                                    var modifier = new DisableAllAbilities();

                                    modifier.AddLifetime(new EndOfStep(Step.EndOfTurn));

                                    return modifier;
                                },
                            };

                            return new AddContiniousEffect(new ContinuousEffect(pr));
                        },

                        () =>
                        {
                            var pr = new ContinuousEffectParameters
                            {
                                Modifier = () =>
                                {
                                    var modifier = new ChangeToCreature(1, 1, "Frog", new[] { CardColor.Blue });

                                    modifier.AddLifetime(new EndOfStep(Step.EndOfTurn));

                                    return modifier;
                                },
                            };

                            return new AddContiniousEffect(new ContinuousEffect(pr));
                        },
                  });

                  p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

                  p.TargetingRule(new EffectBounce());
                  p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
              });
        }
    }
}
