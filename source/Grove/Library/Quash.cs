namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Quash : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quash")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text(
          "Counter target instant or sorcery spell. Search its controller's graveyard, hand, and library for all cards with the same name as that spell and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new CounterTargetSpell(),
              new ExileCardsWithSameNameAsTargetFromGhl());
               
            p.TargetSelector.AddEffect(trg => trg
              .Is.CounterableSpell(e => e.Source.OwningCard.Is().Instant || e.Source.OwningCard.Is().Sorcery)
              .On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}