namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetachedEvent>
  {
    public void Receive(AttachmentDetachedEvent message)
    {
      if (message.AttachedTo == Ability.SourceCard)
        Set(message);
    }
  }
}