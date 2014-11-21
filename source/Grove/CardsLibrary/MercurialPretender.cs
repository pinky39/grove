namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;
  using ReturnToHand = Effects.ReturnToHand;

  public class MercurialPretender : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mercurial Pretender")
        .ManaCost("{4}{U}")
        .Type("Creature - Shapeshifter")
        .Text(
          "You may have Mercurial Pretender enter the battlefield as a copy of any creature you control except it gains \"{2}{U}{U}: Return this creature to its owner's hand.\"")
        .FlavorText("The king went off to find himself. Imagine his terror when he succeeded.")
        .Power(0)
        .Toughness(0)
        .Cast(p => p.TimingRule(new WhenPermanentCountIs(1, c => c.Is().Creature)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "You may have Mercurial Pretender enter the battlefield as a copy of any creature you control except it gains \"{2}{U}{U}: Return this creature to its owner's hand.\"";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.UsesStack = false;

            p.Effect = () => new CompoundEffect(
              new BecomeCopyOfTargetCard(),
              new ApplyModifiersToSelf(() =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{2}{U}{U}: Return this creature to its owner's hand.",
                      Cost = new PayMana("{2}{U}{U}".Parse(), ManaUsage.Abilities),
                      Effect = () => new ReturnToHand(returnOwningCard: true)
                    };

                  ap.TimingRule(new WhenOwningCardWillBeDestroyed());
                  ap.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());

                  return new AddActivatedAbility(new ActivatedAbility(ap));
                })
              );

            p.TargetSelector.AddEffect(trg =>
              {
                // should not be able to target itself because this
                // will cause infinite loop as the trigger of the 
                // copy will trigger again
                trg.Is.Creature(ControlledBy.SpellOwner,
                  canTargetSelf: false).On.Battlefield();

                trg.MustBeTargetable = false;
              });

            p.TargetingRule(new EffectOrCostRankBy(x => -x.Score));            
          });
    }
  }
}