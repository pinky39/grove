namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SoulOfNewPhyrexia : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of New Phyrexia")
        .ManaCost("{6}")
        .Type("Creature — Avatar")
        .Text("{Trample}{EOL}{5}: Permanents you control gain indestructible until end of turn.{EOL}{5}, Exile Soul of New Phyrexia from your graveyard: Permanents you control gain indestructible until end of turn.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Trample)
        .ActivatedAbility(p =>
        {
          p.Text = "{5}: Permanents you control gain indestructible until end of turn.";
          p.Cost = new PayMana("{5}".Parse());

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,            
            modifier: () => new AddSimpleAbility(Static.Indestructible){UntilEot = true}
            ).SetTags(EffectTag.Indestructible);

          p.TimingRule(new Any(
            new BeforeYouDeclareAttackers(), 
            new AfterOpponentDeclaresAttackers(), 
            new WhenOwningCardWillBeDestroyed()));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{5}, Exile Soul of New Phyrexia from your graveyard: Permanents you control gain indestructible until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{5}".Parse()),
            new ExileOwnerCost());

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,            
            modifier: () => new AddSimpleAbility(Static.Indestructible){UntilEot = true}
            ).SetTags(EffectTag.Indestructible);

          p.ActivationZone = Zone.Graveyard;

          p.TimingRule(new Any(
            new BeforeYouDeclareAttackers(),
            new AfterOpponentDeclaresAttackers(),
            new WhenOwningCardWillBeDestroyed()));
        });
    }
  }
}
