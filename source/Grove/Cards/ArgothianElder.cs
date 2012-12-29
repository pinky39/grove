namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
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
            Effect<UntapTargetPermanents>(),
            TargetValidator(
              TargetIs.Card(card => card.Is().Land),
              ZoneIs.Battlefield(),
              minCount: 2,
              maxCount: 2),
            targetSelectorAi: TargetSelectorAi.UntapYourLands(),
            timing: Timings.SecondMain())
        );
    }
  }
}