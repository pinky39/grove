namespace Grove.Gameplay.Modifiers
{
  using Card;
  using Infrastructure;
  using Messages;
  using Targeting;

  public class AttachmentLifetime : Lifetime, IReceive<AttachmentDetached>
  {
    private Card _attachment;
    private Card _attachmentTarget;

    public void Receive(AttachmentDetached message)
    {
      if (_attachmentTarget == message.AttachedTo &&
        message.Attachment == _attachment)
      {
        End();
      }
    }

    public override void Initialize(Modifier modifier, Game game)
    {
      base.Initialize(modifier, game);

      _attachment = modifier.Source;
      _attachmentTarget = modifier.Target.Card();
    }
  }
}