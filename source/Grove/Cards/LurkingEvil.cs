namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

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
              () => new Core.Modifiers.ChangeToCreature(
                power: 4,
                toughness: 4,
                colors: L(CardColor.Black),
                type: "Creature Horror"),
              () => new AddStaticAbility(Static.Flying));

            p.TimingRule(new OwningCardHas(c => c.Is().Enchantment));
            p.TimingRule(new Core.Ai.TimingRules.ChangeToCreature());
          });
    }
  }
}