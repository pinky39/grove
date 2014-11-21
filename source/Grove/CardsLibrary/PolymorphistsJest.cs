namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;

  public class PolymorphistsJest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Polymorphist's Jest")
        .ManaCost("{1}{U}{U}")
        .Type("Instant")
        .Text(
          "Until end of turn, each creature target player controls loses all abilities and becomes a blue Frog with base power and toughness 1/1.")
        .FlavorText("\"The flies were bothering me.\"—Jalira, master polymorphist")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (effect, card) => card.Is().Creature,
              modifiers: new CardModifierFactory[]
                {
                  () => new ChangeToCreature(
                    power: m => 1,
                    toughness: m => 1,
                    colors: L(CardColor.Blue),
                    type: m => m.OwningCard.Type.Change(subTypes: "frog")) {UntilEot = true},
                  () => new DisableAllAbilities(activated: true, simple: true, triggered: true) {UntilEot = true}
                });

            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectOpponent());
          });
    }
  }
}