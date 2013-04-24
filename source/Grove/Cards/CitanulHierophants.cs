﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class CitanulHierophants : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .ContinuousEffect(p =>
          {
            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
            p.Modifier = () =>
              {
                var mp = new ManaAbilityParameters
                  {
                    Cost = new Tap(),
                    Text = "{T}:  Add {G} to your mana pool.",
                    Priority = ManaSourcePriorities.Creature,
                    TapRestriction = true
                  };
                
                mp.ManaAmount(Mana.Green);

                return new AddActivatedAbility(new ManaAbility(mp));
              };
          });
    }
  }
}