namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Exhaustion : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Exhaustion")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Creatures and lands target opponent controls don't untap during his or her next untap step.")
        .FlavorText(
          "The mage felt as though he'd been in the stasis suit for days. Upon his return, he found it was months.")
        .Cast(p =>
          {
            p.Timing = Timings.SecondMain();
            p.Effect = Effect<ApplyModifiersToPlayer>(e =>
              {
                e.Player = e.Players.GetOpponent(e.Controller);
                e.Modifiers(Modifier<AddContiniousEffect>(m =>
                  {
                    m.AddLifetime(Lifetime<EndOfUntapStep>(l =>
                      l.OnlyDuringPlayersUntap = m.Target.Player()));

                    m.Effect =
                      Continuous(c =>
                        {
                          c.ModifierFactory = Modifier<AddStaticAbility>(m1 => m1.StaticAbility = Static.DoesNotUntap);

                          c.CardFilter = (card, effect) =>
                            card.Controller == effect.Target.Player() && (card.Is().Creature || card.Is().Land);
                        });
                  }));
              });
          });
    }
  }
}