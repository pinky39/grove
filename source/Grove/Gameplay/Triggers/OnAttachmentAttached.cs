namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnAttachmentAttached : Trigger, IReceive<AttachmentAttached>
  {
    private readonly Func<Card, bool> _filter;

    private OnAttachmentAttached() {}

    public OnAttachmentAttached(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(AttachmentAttached message)
    {
      if (message.AttachedTo == Ability.SourceCard && _filter(message.Attachment))
        Set(message);
    }
  }
}