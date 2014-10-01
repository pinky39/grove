namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
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
          p.Effect = () => new ApplyModifiersToTargets(
            () => new ChangeToCreature(
              power: m => 1,
              toughness: m => 1,
              colors: L(CardColor.Blue),
              type: m => m.OwningCard.Is().Token 
                  ? "Creature - Token Frog" 
                  : "Creature - Frog") { UntilEot = true },
            () => new DisableAllAbilities { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectBounce());
          p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
        });
    }
  }
}
