namespace Grove.Core.Triggers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      if (message.Attachment == Ability.SourceCard)
        Set(message);
    }
  }
}