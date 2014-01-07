namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class ChimeOfNight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chime of Night")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text("When Chime of Night is put into a graveyard from the battlefield, destroy target nonblack creature.")
        .FlavorText("Many sent to serve Davvol carried such instruments, as if to remind him who their true masters were.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.Opponent));
            p.TimingRule(new OnFirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Chime of Night is put into a graveyard from the battlefield, destroy target nonblack creature.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            
            p.Effect = () => new DestroyTargetPermanents();
            
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black))
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}