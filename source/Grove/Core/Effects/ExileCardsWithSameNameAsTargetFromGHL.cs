namespace Grove.Effects
{
  using System.Linq;
  using Events;

  public class ExileCardsWithSameNameAsTargetFromGhl : Effect
  {
    protected override void ResolveEffect()
    {      
      string targetName = string.Empty;
      Player controller = null;
      
      if (Target.IsCard())
      {
        targetName = Target.Card().Name;
        controller = Target.Card().Controller;
      }
      else if (Target.IsEffect())
      {
        var owningCard = Target.Effect().Source.OwningCard;
        
        targetName = owningCard.Name;
        controller = owningCard.Controller;
      }      

      foreach (var card in controller.Hand.ToList())
      {
        if (card.Name.Equals(targetName))
        {
          card.Exile();
        }
        else
        {
          card.Reveal();
        }
      }

      foreach (var card in controller.Graveyard.ToList())
      {
        if (card.Name.Equals(targetName))
        {
          card.Exile();
        }
      }

      Publish(new PlayerSearchesLibrary(controller));

      foreach (var card in controller.Library.ToList())
      {
        if (card.Name.Equals(targetName))
        {
          card.Exile();
        }
      }
      
      controller.ShuffleLibrary();      
    }
  }
}