namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class DeadDrop : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dead Drop")
        .ManaCost("{9}{B}")
        .Type("Sorcery")
        .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}Target player sacrifices two creatures.")
        .FlavorText("Got a diving lesson{EOL}—Sultai expression meaning{EOL}\"was fed to the crocodiles\"")
        .SimpleAbilities(Static.Delve)
        .Cast(p =>
        {
          p.Effect = () => new TargetPlayerSacrificesPermanents(
              count: 2,
              filter: c => c.Is().Creature,
              text: "Sacrifice two creatures."
              );

          p.TargetSelector.AddEffect(trg => trg.Is.Player());

          p.TargetingRule(new EffectOpponent());
          p.TimingRule(new NonTargetRemovalTimingRule(2));
        });
    }
  }
}
