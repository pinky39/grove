namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  // TODO: Ability needs to be implemented.
  public class HushwingGryff : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hushwing Gryff")
        .ManaCost("{2}{W}")
        .Type("Creature — Hippogriff")
        .Text(
          "{Flash}{I}(You may cast this spell any time you could cast an instant.){/I}{EOL}{Flying}{EOL}Creatures entering the battlefield don't cause abilities to trigger.")
        .FlavorText("An overwhelming sense of calm accompanies the gryffs that wheel above the roofs of Gavony.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flying, Static.Flash);
  //    .ContinuousEffect(p =>
  //    {
  //        p.Modifier = () => new DisableAllAbilities(a => false, s => false,
  //          triggered => triggered.Triggers.Any(t => t is OnZoneChanged && (t as OnZoneChanged).To == Zone.Battlefield));
  //        p.CardFilter = (card, effect) => card.Is().Creature;
  //        p.ApplyOnlyToPermaments = false;
  //    });
    }
  }
}