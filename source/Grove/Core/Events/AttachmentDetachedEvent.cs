namespace Grove.Events
{
  public class AttachmentDetachedEvent
  {
    public readonly Card AttachedTo;
    public readonly Card Attachment;

    public AttachmentDetachedEvent(Card attachment, Card attachedTo)
    {
      Attachment = attachment;
      AttachedTo = attachedTo;
    }
  }
}