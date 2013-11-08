namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

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
            p.Effect = () => new ExileTargetAndCardsWithSameNameFromAllZones();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());            
          });
    }
  }
}