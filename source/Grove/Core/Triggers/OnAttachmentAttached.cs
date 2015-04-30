namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnAttachmentAttached : Trigger, IReceive<AttachmentAttachedEvent>
  {
    private readonly CardSelector _cond;

    private OnAttachmentAttached() {}

    public OnAttachmentAttached(CardSelector cond = null)
    {
      _cond = cond ?? delegate { return true; };
    }

    public void Receive(AttachmentAttachedEvent message)
    {
      if (message.AttachedTo == Ability.SourceCard && _cond(message.Attachment, Ctx))
      {
        Set(message);
      }
    }
  }
}