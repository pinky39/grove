namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      if (message.AttachedTo == Ability.SourceCard)
        Set(message);
    }
  }
}