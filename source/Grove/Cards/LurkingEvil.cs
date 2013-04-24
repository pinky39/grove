﻿namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class LurkingEvil : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lurking Evil")
        .ManaCost("{B}{B}{B}")
        .Type("Enchantment")
        .Text("Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.")
        .FlavorText("Ash is our air, darkness our flesh.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.";
            p.Cost = new PayLife(c => (int) Math.Ceiling(c.Controller.Life/2d));
            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 4,
                toughness: 4,
                colors: L(CardColor.Black),
                type: "Creature Horror"),
              () => new AddStaticAbility(Static.Flying));

            p.TimingRule(new OwningCardHas(c => c.Is().Enchantment));
            p.TimingRule(new Ai.TimingRules.ChangeToCreature());
          });
    }
  }
}