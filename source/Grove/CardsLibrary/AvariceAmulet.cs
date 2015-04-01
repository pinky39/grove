namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AvariceAmulet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Avarice Amulet")
        .ManaCost("{4}")
        .Type("Artifact — Equipment")
        .Text("Equipped creature gets +2/+0 and has vigilance and \"At the beginning of your upkeep, draw a card.\"{EOL}When equipped creature dies, target opponent gains control of Avarice Amulet.{EOL}Equip {2} ({2}: Attach to target creature you control. Equip only as a sorcery.)")
        .FlavorText("")
        .ActivatedAbility(p =>
        {
          p.Text = "Equip {2} ({2}: Attach to target creature you control. Equip only as a sorcery.)";

          p.Cost = new PayMana(2.Colorless());

          p.Effect = () => new Attach(
            () => new AddPowerAndToughness(2, 0),
            () => new AddStaticAbility(Static.Vigilance),
            () =>
            {
              var tp = new TriggeredAbility.Parameters();

              tp.Text = "At the beginning of your upkeep, draw a card.";
              tp.Trigger(new OnStepStart(Step.Upkeep));
              tp.Effect = () => new DrawCards(1);

              return new AddTriggeredAbility(new TriggeredAbility(tp));
            });

          p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());

          p.TargetingRule(new EffectCombatEquipment());
          p.TimingRule(new OnFirstMain());

          p.IsEquip = true;
          p.ActivateAsSorcery = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When equipped creature dies, target opponent gains control of Avarice Amulet.";

          p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard,
            selector: (c, ctx) => c == ctx.OwningCard.AttachedTo));

          p.Effect = () => new SwitchController();

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
