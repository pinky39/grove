namespace Grove.Gameplay.Messages
{
  using Card;

  public class AttachmentDetached
  {
    public Card AttachedTo { get; set; }
    public Card Attachment { get; set; }
  }
}