namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class PeelFromReality : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Peel from Reality")
              .ManaCost("{1}{U}")
              .Type("Instant")
              .Text("Return target creature you control and target creature you don't control to their owners' hands.")
              .FlavorText("\"Soulless demon, you are bound to me. Now we will both dwell in oblivion.\"")
              .Cast(p =>
              {
                  p.Text = "Return target creature you control and target creature you don't control to their owners' hands.";

                  p.Effect = () => new ReturnToHand();

                  p.TargetSelector.AddEffect(trg =>
                  {
                      trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                      trg.Message = "Select a target creature you control.";
                  });
                  p.TargetSelector.AddEffect(trg =>
                  {
                      trg.Is.Creature(ControlledBy.Opponent).On.Battlefield();
                      trg.Message = "Select a target creature your oppenent controls.";
                  });

                  p.TargetingRule(new EffectBounce());
                  p.TimingRule(new Any(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly), new WhenYourLifeCanBecomeZero(), 
                      new WhenOpponentControllsPermanents(c => c.CountersCount() > 1 || c.HasAttachments))); 
              });
        }
    }
}
