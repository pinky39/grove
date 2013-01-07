namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class CitanulHierophants : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Citanul Hierophants")
        .ManaCost("{3}{G}")
        .Type("Creature Human Druid")
        .Text("Creatures you control have '{T}: Add {G} to your mana pool.'")
        .FlavorText(
          "From deep in the caves beneath the forest, the hierophants planned the druids' raids against the enemy.")
        .Power(3)
        .Toughness(2)
        .Abilities(
          Continuous(e =>
            {
              e.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
              e.ModifierFactory = Modifier<AddActivatedAbility>(m =>
                m.Ability = ManaAbility(ManaUnit.Green, "{T}: Add {G} to your mana pool.")
                );
            }));
    }
  }
}