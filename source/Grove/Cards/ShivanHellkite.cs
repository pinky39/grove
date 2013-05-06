namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.RepetitionRules;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class ShivanHellkite : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shivan Hellkite")
        .ManaCost("{5}{R}{R}")
        .Type("Creature Dragon")
        .Text("{Flying}{EOL}{1}{R}: Shivan Hellkite deals 1 damage to target creature or player.")
        .FlavorText(
          "A dragon's scale can be carved into a mighty shield, provided you can procure a dragontooth to cut it.")
        .Power(5)
        .Toughness(5)
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{R}: Shivan Hellkite deals 1 damage to target creature or player.";
            p.Cost = new PayMana("{1}{R}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new DealDamageToTargets(1);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new DealDamage(p1 => p1.MaxRepetitions));
            p.TimingRule(new TargetRemoval());
            p.RepetitionRule(new TargetLifepointsLeft());
          });
    }
  }
}