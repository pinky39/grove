﻿namespace Grove.Gameplay.Card.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetached>
  {
    public void Receive(AttachmentDetached message)
    {
      if (message.Attachment == Ability.SourceCard)
        Set(message);
    }
  }
}