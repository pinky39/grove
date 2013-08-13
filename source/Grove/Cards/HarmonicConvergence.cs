namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class HarmonicConvergence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Harmonic Convergence")
        .ManaCost("{2}{G}")
        .Type("Instant")
        .Text("Put all enchantments on top of their owners' libraries.")
        .FlavorText("When the eternal stars align, can mere mortals resist?")
        .Cast(p =>
          {
            p.Effect = () => new PutAllPermanentsOnTopOfLibrary(c => c.Is().Enchantment);
            p.TimingRule(new Any(new ModifyAttackersAndBlockers(), new EndOfTurn()));
          });
    }
  }
}