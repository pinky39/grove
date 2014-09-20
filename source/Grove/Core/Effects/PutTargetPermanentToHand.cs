namespace Mtg.Core.Effects
{
  public class PutTargetPermanentToHand : Effect
  {
    public int Discard = 0;
    
    public override void Resolve()
    {
      Target.Card().ReturnToHand();

      if (Discard > 0)
      {
        Decisions.EnqueueDiscardCards(Target.Card().Controller, Discard);
      }
    }
  }
}