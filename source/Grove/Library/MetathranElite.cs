namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class MetathranElite : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Metathran Elite")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Metathran Soldier")
        .Text("Metathran Elite can't be blocked as long as it's enchanted.")
        .FlavorText("If the eyes are windows to the soul, their souls are not of this world.")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnAttachmentAttached(c => c.Is().Aura));
            
            p.Effect = () => new ApplyModifiersToSelf(() =>
              {
                var modifier = new AddStaticAbility(Static.Unblockable);
                
                modifier.AddLifetime(new AttachmentLifetime(self => 
                  self.Modifier.SourceEffect.TriggerMessage<AttachmentAttached>().Attachment));

                return modifier;
              });
            
            p.UsesStack = false;
          });
    }
  }
}