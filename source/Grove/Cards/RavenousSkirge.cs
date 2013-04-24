namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class RavenousSkirge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Ravenous Skirge")
        .ManaCost("{2}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}Whenever Ravenous Skirge attacks, it gets +2/+0 until end of turn.")
        .FlavorText("Hunger is a kind of madness—and here, all madness flourishes.")
        .Power(1)
        .Toughness(1)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Ravenous Skirge attacks, it gets +2/+0 until end of turn.";
            p.Trigger(new OnAttack());
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 0) { UntilEot = true });
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}