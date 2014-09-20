namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Opposition : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opposition")
        .ManaCost("{2}{U}{U}")
        .Type("Enchantment")
        .Text("Tap an untapped creature you control: Tap target artifact, creature, or land.")
        .FlavorText("Urza says he's sane. Perhaps, but measures of sanity among planeswalkers are hard to come by.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Tap an untapped creature you control: Tap target artifact, creature, or land.";
            p.Cost = new Tap();
            p.Effect = () => new TapTargets();

            p.TargetSelector
              .AddCost(trg =>
                {
                  trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                  trg.Message = "Select a creature you control.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.Card(c => c.Is().Creature || c.Is().Artifact || c.Is().Land).On.Battlefield();
                  trg.Message = "Select an artifact, creature or land.";
                });

            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
            p.TargetingRule(new CostTapEffectTap());
          });
    }
  }
}