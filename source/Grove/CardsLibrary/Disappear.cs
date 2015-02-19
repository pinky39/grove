namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Disappear : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disappear")
        .ManaCost("{2}{U}{U}")
        .Type("Enchantment Aura")
        .Text("{U}: Return enchanted creature and Disappear to their owners' hands.")
        .FlavorText("If at first you don't succeed, run away and hide.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectBounce());            
          })
        .ActivatedAbility(p =>
          {
            p.Cost = new PayMana(Mana.Blue);
            p.Text = "{U}: Return enchanted creature and Disappear to their owners' hands.";
            p.Effect = () => new ReturnOwnerAndAttachedToHand();

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new BeforeYouDeclareAttackers()));
          });
    }
  }
}