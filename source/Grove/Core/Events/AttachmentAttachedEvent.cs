namespace Grove.Events
{
  public class AttachmentAttachedEvent
  {
    public readonly Card Attachment;

    public AttachmentAttachedEvent(Card attachment)
    {
      Attachment = attachment;
    }

    public Card AttachedTo { get { return Attachment.AttachedTo; } }
  }
}