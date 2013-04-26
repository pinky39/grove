namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.CostRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

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
            p.Cost = new PayMana(Mana.Zero, ManaUsage.Abilities, hasX: true);

            p.Effect = () => new ApplyModifiersToSelf(() =>
              new ChangeToCreature(
                power: Value.PlusX, toughness: Value.PlusX,
                type: "Creature Artifact Construct") {UntilEot = true});

            p.TimingRule(new Ai.TimingRules.ChangeToCreature(minAvailableMana: 3));
            p.CostRule(new MaxAvailableMana());
          }
        );
    }
  }
}