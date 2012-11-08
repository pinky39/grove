namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class EndlessWurm : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Endless Wurm")
        .ManaCost("{3}{G}{G}")
        .Type("Creature Wurm")
        .Text("{Trample}{EOL}At the beginning of your upkeep, sacrifice Endless Wurm unless you sacrifice an enchantment.")
        .FlavorText("Ages ago, a party of elves took cover to let one pass. They're still waiting.")
        .Power(9)
        .Toughness(9)
        .Timing(Timings.Creatures())
        .Abilities(          
          Static.Trample,
          TriggeredAbility(
            "At the beginning of your upkeep, sacrifice Endless Wurm unless you sacrifice an enchantment.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.Upkeep; }),
            Effect<SacPermanentOrSacrificeOwner>(e =>
              {
                e.PayUpkeepAi = (owner) => owner.IsAbleToAttack;
                e.Selector = card => card.Is().Enchantment;
                e.Message = "Select enchantment to sacrifice";
              }),
            triggerOnlyIfOwningCardIsInPlay: true));          
    }
  }
}