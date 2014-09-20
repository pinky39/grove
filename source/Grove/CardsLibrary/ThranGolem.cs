namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class ThranGolem : CardTemplateSource
  {
    private static Lifetime UntilAttached()
    {
      return new AttachmentLifetime(self =>
        self.Modifier.SourceEffect.TriggerMessage<AttachmentAttachedEvent>().Attachment);
    }

    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Golem")
        .ManaCost("{5}")
        .Type("Artifact Creature Golem")
        .Text("As long as Thran Golem is enchanted, it gets +2/+2 and has flying, first strike, and trample.")
        .FlavorText("Karn felt more secure about his value to Urza when he realized he didn't need regular trimming.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnAttachmentAttached(c => c.Is().Aura));
            p.Effect = () =>
              {
                return new ApplyModifiersToSelf(
                  () =>
                    {
                      var modifier = new AddPowerAndToughness(2, 2);
                      modifier.AddLifetime(UntilAttached());
                      return modifier;
                    },
                  () =>
                    {
                      var modifier = new AddStaticAbility(Static.Flying);
                      modifier.AddLifetime(UntilAttached());
                      return modifier;
                    },
                  () =>
                    {
                      var modifier = new AddStaticAbility(Static.FirstStrike);
                      modifier.AddLifetime(UntilAttached());
                      return modifier;
                    },
                  () =>
                    {
                      var modifier = new AddStaticAbility(Static.Trample);
                      modifier.AddLifetime(UntilAttached());
                      return modifier;
                    });
              };

            p.UsesStack = false;
          });
    }
  }
}