namespace Grove.CardsLibrary
{
  using System;
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class LurkingEvil : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lurking Evil")
        .ManaCost("{B}{B}{B}")
        .Type("Enchantment")
        .Text("Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.")
        .FlavorText("Ash is our air, darkness our flesh.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror creature with flying.";
            p.Cost = new PayLife(c => (int) Math.Ceiling(c.Controller.Life/2d));
            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 4,
                toughness: 4,
                colors: L(CardColor.Black),
                type: t => t.Change(baseTypes: "creature", subTypes: "horror")),
              () => new AddSimpleAbility(Static.Flying));

            p.TimingRule(new WhenCardHas(c => c.Is().Enchantment));
            p.TimingRule(new BeforeYouDeclareAttackers());
          });
    }
  }
}