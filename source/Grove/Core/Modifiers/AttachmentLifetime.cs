namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class AttachmentLifetime : Lifetime, IReceive<AttachmentDetached>
  {
    public AttachmentLifetime(Modifier modifier, ChangeTracker changeTracker) : base(modifier, changeTracker) { }
    private AttachmentLifetime () {}

    public void Receive(AttachmentDetached message)
    {
      if (ModifierTarget == message.AttachedTo &&
        message.Attachment == ModifierSource)
      {
        End();
      }
    }
  }
}