namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class VolcanicFallout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Volcanic Fallout")
        .ManaCost("{1}{R}{R}")
        .Type("Instant")
        .Text(
          "Volcanic Fallout can't be countered.{EOL}Volcanic Fallout deals 2 damage to each creature and each player.")
        .FlavorText("How can we outrun the sky?")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountPlayer: 2,
              amountCreature: 2) {CanBeCountered = false};

            p.TimingRule(new MassRemovalTimingRule());
          });
    }
  }
}