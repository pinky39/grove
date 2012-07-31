namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Targeting;

  public class ReturnToHand : Effect
  {
    public int Discard;
    public bool ReturnTarget;
    public bool ReturnOwner;

    public override bool NeedsTargets { get { return ReturnTarget; } }

    protected override void ResolveEffect()
    {
      if (ReturnTarget)
      {
        Target().Card().ReturnToHand();
      }

      if (ReturnOwner && !(ReturnTarget && Target() == Source.OwningCard))
      {
        Source.OwningCard.ReturnToHand();
      }

      if (Discard > 0)
      {
        Decisions.Enqueue<DiscardCards>(
          controller: Target().Card().Controller,
          init: p => p.Count = Discard);
      }
    }
  }
}