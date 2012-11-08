namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class ArgothianElder : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Elder")
        .ManaCost("{3}{G}")
        .Type("Creature Elf Druid")
        .Text("{T}: Untap two target lands.")
        .FlavorText("Sharpen your ears{EOL}—Elvish expression meaning 'grow wiser'")
        .Power(2)
        .Toughness(2)
        .Abilities(
          ActivatedAbility(
            "{T}: Untap two target lands.",
            Cost<TapOwnerPayMana>(cost => cost.TapOwner = true),
            Effect<UntapTargets>(),
            effectValidator: Validator(Validators.Permanent(card => card.Is().Land), minCount: 2, maxCount: 2),
            selectorAi: TargetSelectorAi.UntapYourLands(),
            timing: Timings.SecondMain()
            )
        );
    }
  }
}