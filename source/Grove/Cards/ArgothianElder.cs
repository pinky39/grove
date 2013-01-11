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
            Cost<Tap>(),
            Effect<UntapTargetPermanents>(),
            Target(
              Validators.Card(card => card.Is().Land),
              Zones.Battlefield(),
              minCount: 2,
              maxCount: 2),
            targetingAi: TargetingAi.UntapYourLands(),
            timing: Timings.SecondMain())
        );
    }
  }
}