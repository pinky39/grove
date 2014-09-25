namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class MercurialPretender : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Mercurial Pretender")
              .ManaCost("{4}{U}")
              .Type("Creature - Shapeshifter")
              .Text("You may have Mercurial Pretender enter the battlefield as a copy of any creature you control except it gains \"{2}{U}{U}: Return this creature to its owner's hand.\"")
              .FlavorText("The king went off to find himself. Imagine his terror when he succeeded.")
              .Power(0)
              .Toughness(0)
              .Cast(p => { p.PutToZoneAfterResolve = c => c.ChangeZone(new NullZone(), delegate { }); })
              .TriggeredAbility(p =>
              {
                  p.Text = "You may have Mercurial Pretender enter the battlefield as a copy of any creature you control except it gains \"{2}{U}{U}: Return this creature to its owner's hand.\"";

                  p.Trigger(new OnZoneChanged(to: Zone.Stack));
                  
                  p.Effect = () => new CreateCopyOfTargetCreature(null,
                      (c, g) =>
                      {
                          c.PutToBattlefield();
//                          c.Text += "{2}{U}{U}: Return this creature to its owner's hand.";
                      },
                      () =>
                      {
                          var tp = new ActivatedAbilityParameters();
                          tp.Text = "{2}{U}{U}: Return this creature to its owner's hand.";
                          tp.Cost = new PayMana("{2}{U}{U}".Parse(), ManaUsage.Abilities);
                          tp.Effect = () => new CreateCopyOfTargetCreature(
                              Cards.Create("Mercurial Pretender"),
                              (c, g) => c.PutToHand());
                          tp.PutToZoneAfterActivation = c => c.ChangeZone(new NullZone(), delegate { });
                          
                          return new AddActivatedAbility(new ActivatedAbility(tp));
                      });

                  p.UsesStack = false;

                  p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());

                  p.TimingRule(new OnFirstMain());
//                  p.TimingRule(new WhenPermanentCountIs(c => c.Is().Creature));
                  p.TargetingRule(new EffectCombatEnchantment());
              });
        }
    }
}
