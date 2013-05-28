namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      if (message.Attachment == Ability.SourceCard)
        Set(message);
    }
  }
}