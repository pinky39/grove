namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.CostRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

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
              new ChangeToCreature(
                power: Value.PlusX, toughness: Value.PlusX,
                type: t => t.Add(baseTypes: "artifact creature", subTypes: "construct")) {UntilEot = true});

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