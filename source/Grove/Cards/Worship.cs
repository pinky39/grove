namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Dsl;

  public class Worship : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "If you control a creature, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("'Believe in the ideal, not the idol.'{EOL}—Serra")
        .Cast(p => p.Timing = All(Timings.FirstMain(), Timings.OnlyOneOfKind()))        
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddDamagePrevention>(
                m => m.Prevention = Prevention<PreventLifelossBelowOne>());              
              e.PlayerFilter = (player, effect) => player == effect.Source.Controller;
            })
        );
    }
  }
}