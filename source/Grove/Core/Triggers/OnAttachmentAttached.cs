namespace Grove.Triggers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnAttachmentAttached : Trigger, IReceive<AttachmentAttachedEvent>
  {
    private readonly Func<Card, bool> _filter;

    private OnAttachmentAttached() {}

    public OnAttachmentAttached(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(AttachmentAttachedEvent message)
    {
      if (message.AttachedTo == Ability.SourceCard && _filter(message.Attachment))
        Set(message);
    }
  }
}