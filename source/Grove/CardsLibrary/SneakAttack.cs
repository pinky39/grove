namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SneakAttack : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sneak Attack")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text(
          "{R}: You may put a creature card from your hand onto the battlefield. That creature gains haste. Sacrifice the creature at the beginning of the next end step.")
        .FlavorText("Nothin' beat surprise—'cept rock.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}: You may put a creature card from your hand onto the battlefield. That creature gains haste. Sacrifice the creature at the beginning of the next end step.";
            p.Cost = new PayMana(Mana.Red);

            p.Effect = () => new PutSelectedCardsToBattlefield(
              text: "Select a creature card in your hand.",
              validator: c => c.Is().Creature,
              fromZone: Zone.Hand,
              modifiers: L(
                () => new AddSimpleAbility(Static.Haste) {UntilEot = true},
                () =>
                  {
                    var tp = new TriggeredAbility.Parameters
                      {
                        Text = "Sacrifice the creature at the beginning of the next end step.",
                        Effect = () => new SacrificeOwner(),
                      };

                    tp.Trigger(new OnStepStart(
                      step: Step.EndOfTurn,
                      passiveTurn: true,
                      activeTurn: true));

                    tp.UsesStack = false;
                    return new AddTriggeredAbility(new TriggeredAbility(tp));
                  }));

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}