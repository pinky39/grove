namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class RainOfFilth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rain of Filth")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Until end of turn, lands you control gain 'Sacrifice this land: Add {B} to your mana pool.'")
        .FlavorText("When I say it rained, it was not small drops, but a thick, greasy drool pouring from the heavens.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (e, c) => c.Is().Land,
              controlledBy: ControlledBy.SpellOwner,
              modifiers: () =>
                {
                  var mp = new ActivatedAbilityParameters
                    {
                      Cost = new Sacrifice(),
                      Text = "Sacrifice this land: Add {B} to your mana pool.",
                      Effect = () => new AddManaToPool("{B}".Parse()),
                      UsesStack = false,
                    };

                  mp.TimingRule(new WhenYouNeedAdditionalMana());
                  
                  return new AddActivatedAbility(new ActivatedAbility(mp));
                });

            p.TimingRule(new OnYourTurn(Step.Upkeep));
          });
    }
  }
}