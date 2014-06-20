namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;  

  public class Eradicate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Eradicate")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text(
          "Exile target nonblack creature. Search its controller's graveyard, hand, and library for all cards with the same name as that creature and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ExileCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black)).On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());
          });
    }
  }
}