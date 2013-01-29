namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class AttachmentLifetime : Lifetime, IReceive<AttachmentDetached>
  {
    private readonly Card _attachment;
    private readonly Card _attachmentTarget;

    public AttachmentLifetime(Card attachment, Card attachmentTarget)
    {
      _attachment = attachment;
      _attachmentTarget = attachmentTarget;
    }

    public void Receive(AttachmentDetached message)
    {
      if (_attachmentTarget == message.AttachedTo &&
        message.Attachment == _attachment)
      {
        End();
      }
    }
  }
}