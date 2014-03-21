namespace Grove.Effects
{
  using AI;

  public class ReturnOwnerAndAttachedToHand : Effect
  {
    public ReturnOwnerAndAttachedToHand()
    {
      SetTags(EffectTag.Bounce);
    }
    
    protected override void ResolveEffect()
    {
      var owningCard = Source.OwningCard;
      var attachedTo = owningCard.AttachedTo;
            
      owningCard.PutToHandFrom(Zone.Battlefield);
      
      if (attachedTo != null)
      {
        attachedTo.PutToHandFrom(Zone.Battlefield);
      }             
    }
  }
}