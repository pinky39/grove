namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Modifiers;
  using Core.Preventions;

  // fix worship Worship does not prevent damage. It causes some damage to be unable to lower your life total. So any damage rendered useless by Worship was still dealt and is counted by effects that track the amount of damage done to a player. In addition, Worship does not prevent loss of life, so loss of life bypasses Worship.
  public class Worship : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "If you control a creature, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("'Believe in the ideal, not the idol.'{EOL}—Serra")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddDamagePrevention>(
                (m, c0) => m.Prevention = c0.Prevention<PreventDamageFromAnySource>(
                  (p, _) => p.Amount = (self, damageAmount) =>
                    {
                      var controller = self.Owner.Player();
                      var controlsCreature = controller.Battlefield.Creatures.Any();

                      if (!controlsCreature)
                        return 0;

                      var lifeAfterDamage = controller.Life - damageAmount;

                      if (lifeAfterDamage < 1)
                      {
                        return damageAmount - controller.Life + 1;
                      }

                      return 0;
                    }));
              e.CardFilter = delegate { return false; };
              e.PlayerFilter = (player, armor) => player == armor.Controller;
            })
        );
    }
  }
}