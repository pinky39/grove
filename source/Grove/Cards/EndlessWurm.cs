namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class EndlessWurm : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .StaticAbilities(Static.Trample)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, sacrifice Endless Wurm unless you sacrifice an enchantment.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new SacrificePermanentOrSacrificeOwner(
              validator: c => c.Is().Enchantment,
              shouldPayAi: (controller, card) => card.IsAbleToAttack,
              text: "Pay upkeep, select an enchantment to sacrifice.");
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}