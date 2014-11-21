namespace Grove.Effects
{
  using Events;
  using Modifiers;

  public class BecomeCopyOfTargetCard : Effect
  {
    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var target = (Card) Target;

      var modifier = new ChangeCardTemplate(target.Template);
      Source.OwningCard.AddModifier(modifier, p);

      // this will trigger  comes into play triggered abilities 
      // of the copy
      Publish(new ZoneChangedEvent(Source.OwningCard,
        Zone.Stack,
        Zone.Battlefield));
    }
  }
}