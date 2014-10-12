namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class DiffusionSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Diffusion Sliver")
        .ManaCost("{1}{U}")
        .Type("Creature — Sliver")
        .Text(
          "Whenever a Sliver creature you control becomes the target of a spell or ability an opponent controls, counter that spell or ability unless its controller pays {2}.")
        .FlavorText("\"The hive shimmered, and its walls seemed a living mirror.\"{EOL}—Hastric, Thunian scout ")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a Sliver creature you control becomes the target of a spell or ability an opponent controls, counter that spell or ability unless its controller pays {2}.";
          
          p.Trigger(new OnBeingTargetedBySpellOrAbility((target, effect, trigger) =>
          {
            if (effect.Controller == trigger.Controller)
              return false;
          
            if (target.IsCard() && target.Card().Is("Sliver") && target.Card().Controller == trigger.Controller)
            {
              return true;
            }
          
            return false;
          }));

          p.UsesStack = false;
          p.TriggerOnlyIfOwningCardIsInPlay = true;          
          
          p.Effect = () => new CounterTopSpell(doNotCounterCost: 2);
        });
    }
  }
}
