namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class EndlessWurm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Endless Wurm")
        .ManaCost("{3}{G}{G}")
        .Type("Creature Wurm")
        .Text(
          "{Trample}{EOL}At the beginning of your upkeep, sacrifice Endless Wurm unless you sacrifice an enchantment.")
        .FlavorText("Ages ago, a party of elves took cover to let one pass. They're still waiting.")
        .Power(9)
        .Toughness(9)
        .SimpleAbilities(Static.Trample)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, sacrifice Endless Wurm unless you sacrifice an enchantment.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new SacrificePermanentOrSacrificeOwner(
              validator: c => c.Is().Enchantment,
              shouldPayAi: (controller, card) => card.IsAbleToAttack,
              text: "Select an enchantment to sacrifice.",
              instructions: "(Press Enter to sacrifice Endless wurm.)");
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}