namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class FledglingOsprey : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fledgling Osprey")
        .ManaCost("{U}")
        .Type("Creature Bird")
        .Text("Fledgling Osprey has flying as long as it's enchanted.")
        .FlavorText("It isn't truly born until its first flight.")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnAttachmentAttached(c => c.Is().Aura));
            
            p.Effect = () => new ApplyModifiersToSelf(() =>
              {
                var modifier = new AddStaticAbility(Static.Flying);
                
                modifier.AddLifetime(new AttachmentLifetime(self => 
                  self.Modifier.SourceEffect.TriggerMessage<AttachmentAttached>().Attachment));

                return modifier;
              });
            
            p.UsesStack = false;
          });
    }
  }
}