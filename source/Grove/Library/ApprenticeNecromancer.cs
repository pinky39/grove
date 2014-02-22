namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class ApprenticeNecromancer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Apprentice Necromancer")
        .ManaCost("{1}{B}")
        .Type("Creature Zombie Wizard")
        .Text(
          "{B},{T}, Sacrifice Apprentice Necromancer: Return target creature card from your graveyard to the battlefield. That creature gains haste. At the beginning of the next end step, sacrifice it.")        
        .Power(1)
        .Toughness(1)        
        .ActivatedAbility(p =>
          {
            p.Text =
              "{B},{T},Sacrifice Apprentice Necromancer: Return target creature card from your graveyard to the battlefield. That creature gains haste. At the beginning of the next end step, sacrifice it.";
            
            p.Cost = new AggregateCost(
              new PayMana(Mana.Black, ManaUsage.Abilities),
              new Tap(),
              new Sacrifice());

            p.Effect = () => new PutSelectedCardToBattlefield(
              "Select a creature card in your graveyard.",
              c => c.Is().Creature,
              Zone.Graveyard,
              () => new AddStaticAbility(Static.Haste) {UntilEot = true},
              () =>
                {
                  var tp = new TriggeredAbilityParameters
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
                });

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}