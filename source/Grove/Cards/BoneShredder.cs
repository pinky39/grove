namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class BoneShredder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bone Shredder")
        .ManaCost("{2}{B}")
        .Type("Creature Minion")
        .Text("{Flying}, {Echo} {2}{B}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}When Bone Shredder enters the battlefield, destroy target nonartifact, nonblack creature.")
        .Power(1)
        .Toughness(1)
        .Echo("{2}{B}")
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Bone Shredder enters the battlefield, destroy target nonartifact, nonblack creature.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black) && !c.Is().Artifact)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}