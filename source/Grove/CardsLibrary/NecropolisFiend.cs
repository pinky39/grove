namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.CostRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class NecropolisFiend : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Necropolis Fiend")
          .ManaCost("{7}{B}{B}")
          .Type("Creature — Demon")
          .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}{Flying}{EOL}{X}, {T}, Exile X cards from your graveyard: Target creature gets -X/-X until end of turn.")
          .Power(4)
          .Toughness(5)
          .SimpleAbilities(Static.Flying, Static.Delve)
          .ActivatedAbility(p =>
          {
            p.Text = "{X}, {T}, Exile X cards from your graveyard: Target creature gets -X/-X until end of turn.";
            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, ManaUsage.Abilities, hasX: true),
              new Tap());

            p.Effect = () => new CompoundEffect(
              new ExileSelectedCards(Value.PlusX, Zone.Graveyard),
              new ApplyModifiersToTargets(() => new AddPowerAndToughness(Value.MinusX, Value.MinusX) { UntilEot = true }) { ToughnessReduction = Value.PlusX });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness());
            p.CostRule(new XIsGraveyardCardCount());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
          }); ;
    }
  }
}
