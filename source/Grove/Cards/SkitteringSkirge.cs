namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class SkitteringSkirge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Skittering Skirge")
        .ManaCost("{B}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}When you cast a creature spell, sacrifice Skittering Skirge.")
        .FlavorText(
          "The imps' warbling cries echo through Phyrexia's towers like those of mourning doves in a cathedral.")
        .Power(3)
        .Toughness(2)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnCastedSpell((a, c) =>
              a.OwningCard.Controller == c.Controller && c.Is().Creature));

            p.Effect = () => new SacrificeSource();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}