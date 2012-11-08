namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

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
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.CardFilter = (card, source) => card.Controller == source.Controller && card.Is().Creature;
              e.ModifierFactory = Modifier<AddActivatedAbility>(m =>
                m.Ability = ManaAbility(ManaUnit.Green, "{T}: Add {G} to your mana pool.")
                );
            }));
    }
  }
}