namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      if (message.AttachedTo == Ability.SourceCard)
        Set(message);
    }
  }
}