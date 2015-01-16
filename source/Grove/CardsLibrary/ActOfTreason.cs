namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class ActOfTreason : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Act of Treason")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text("Gain control of target creature until end of turn. Untap that creature. It gains haste until end of turn.")
        .FlavorText("\"The Sultai take our dead, so we shall take their living!\"{EOL}—Taklai, Mardu ragesinger")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new ApplyModifiersToTargets(
              () => new ChangeController(m => m.SourceCard.Controller) { UntilEot = true },
              () => new AddStaticAbility(Static.Haste) { UntilEot = true }),
            new UntapTargetPermanents());

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectGainControl());
        });
    }
  }
}
