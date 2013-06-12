namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

  public class AttachmentLifetime : Lifetime, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      var attachment = Modifier.SourceCard;      

      if (message.Attachment == attachment)
      {
        End();
      }
    }
  }
}