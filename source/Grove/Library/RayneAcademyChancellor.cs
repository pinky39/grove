namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class RayneAcademyChancellor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rayne, Academy Chancellor")
        .ManaCost("{2}{U}")
        .Type("Legendary Creature Human Wizard")
        .Text("Whenever you or a permanent you control becomes the target of a spell or ability an opponent controls, you may draw a card. You may draw an additional card if Rayne, Academy Chancellor is enchanted.")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you or a permanent you control becomes the target of a spell or ability an opponent controls, you may draw a card. You may draw an additional card if Rayne, Academy Chancellor is enchanted.";

            p.Trigger(new OnBeingTargetedBySpellOrAbility((target, effect, trigger) =>
              {
                if (effect.Controller == trigger.Controller)
                  return false;
                
                if ((target.IsCard() && target.Card().Controller == trigger.Controller) ||                
                  trigger.Controller == target)
                {
                  return true;
                }

                return false;
              }));

            p.Effect = () => new DrawCards(P(e =>
              {
                if (e.Source.OwningCard.IsEnchanted)
                  return 2;

                return 1;
              }, evaluateOnResolve: true));
              
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}