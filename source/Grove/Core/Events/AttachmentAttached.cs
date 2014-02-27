namespace Grove.Events
{
  public class AttachmentAttached
  {
    public Card AttachedTo { get { return Attachment.AttachedTo; }}
    public Card Attachment { get; set; }
  }
}