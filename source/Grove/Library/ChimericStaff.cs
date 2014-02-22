namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.CostRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

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

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Creature));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
            p.CostRule(new XIsAvailableMana());
          }
        );
    }
  }
}