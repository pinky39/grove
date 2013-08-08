namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class ChimericStaff : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              new Gameplay.Modifiers.ChangeToCreature(
                power: Value.PlusX, toughness: Value.PlusX,
                type: "Creature Artifact Construct") {UntilEot = true});

            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new OwningCardHas(c => !c.Is().Creature));
            p.TimingRule(new Artifical.TimingRules.ChangeToCreature(minAvailableMana: 3));
            p.CostRule(new MaxAvailableMana());
          }
        );
    }
  }
}