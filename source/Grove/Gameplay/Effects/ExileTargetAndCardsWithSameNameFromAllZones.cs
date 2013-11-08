namespace Grove.Gameplay.Effects
{
  using System.Linq;
  using Targeting;

  public class ExileTargetAndCardsWithSameNameFromAllZones : Effect
  {
    protected override void ResolveEffect()
    {
      var target = Target.Card();
      var targetName = target.Name;
      var controller = target.Controller;      
            
      target.Exile();

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