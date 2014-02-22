namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Costs;

  public class MartyrsCause : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Martyr's Cause")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "Sacrifice a creature: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.")
        .FlavorText("Dying is a soldier's talent.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice a creature: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.";

            p.Cost = new Sacrifice();
            p.Effect = () => new Gameplay.Effects.PreventDamageFromSourceToTarget();

            p.TargetSelector.AddCost(trg =>
              trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());

            p.TargetSelector
              .AddEffect(trg =>
                {
                  trg.Is.Card().On.BattlefieldOrStack();
                  trg.Message = "Select damage source.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.CreatureOrPlayer().On.Battlefield();
                  trg.Message = "Select creature or player.";
                });

            p.TargetingRule(new CostSacrificeEffectPreventDamageFromSourceToTarget());
          });
    }
  }
}