namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class LurkingEvil : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lurking Evil")
        .ManaCost("{B}{B}{B}")
        .Type("Enchantment")
        .Text("Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.")
        .FlavorText("'Ash is our air, darkness our flesh.'{EOL}—Phyrexian Scriptures")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          ActivatedAbility(
            "Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.",
            Cost<PayLife>(cost => cost.GetAmount = self => (int) Math.Ceiling(self.Controller.Life/2d)),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 4;
                  m.Toughness = 4;
                  m.Colors = ManaColors.Black;
                  m.Type = "Creature - Horror";
                }),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying))),
            timing: All(Timings.ChangeToCreature(), Timings.Has(c => c.Is().Enchantment)))
        );
    }
  }
}