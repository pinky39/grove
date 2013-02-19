namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.CostRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class ChimericStaff : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Chimeric Staff")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{X}: Chimeric Staff becomes an X/X Construct artifact creature until end of turn.")
        .FlavorText("A snake in the grasp.")
        .ActivatedAbility(p =>
          {
            p.Text = "{X}: Chimeric Staff becomes an X/X Construct artifact creature until end of turn.";
            p.Cost = new PayMana(ManaAmount.Zero, ManaUsage.Abilities, hasX: true);
            
            p.Effect = () => new ApplyModifiersToSelf(() =>
              new ChangeToCreature(
                power: Value.PlusX, toughness: Value.PlusX,
                type: "Creature Artifact Construct") {UntilEot = true});

            p.TimingRule(new Core.Ai.TimingRules.ChangeToCreature(minAvailableMana: 3));
            p.CostRule(new MaxAvailableMana(ManaUsage.Abilities));
          }
        );
    }
  }
}