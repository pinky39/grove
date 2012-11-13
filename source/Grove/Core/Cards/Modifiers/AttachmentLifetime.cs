namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class AttachmentLifetime : Lifetime, IReceive<AttachmentDetached>
  {
    public Card Attachment { get; set; }
    public Card AttachmentTarget { get; set; }        

    public void Receive(AttachmentDetached message)
    {
      if (AttachmentTarget == message.AttachedTo &&
        message.Attachment == Attachment)
      {
        End();
      }
    }
  }
}