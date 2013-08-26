namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

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
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 4,
                toughness: 4,
                colors: L(CardColor.Black),
                type: "Creature Horror"),
              () => new AddStaticAbility(Static.Flying));

            p.TimingRule(new WhenCardHas(c => c.Is().Enchantment));
            p.TimingRule(new Artifical.TimingRules.BeforeYouDeclareAttackers());
          });
    }
  }
}