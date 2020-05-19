namespace Grove.Effects
{
  using Grove.Modifiers;
  using System.Linq;

  public class PutCreaturesFromGraveyardsToYourBattlefield : Effect
  {
    protected override void ResolveEffect()
    {
      var creatures = Players.SelectMany(x => x.Graveyard).Where(x => x.Is().Creature).ToList(); 

      foreach (var card in creatures)
      {
        if (card.Owner != Controller)
        {
          var modifier = new ChangeController(Controller);

          var p = new ModifierParameters
          {
            SourceEffect = this,
            SourceCard = Source.OwningCard,
            X = X
          };

          card.AddModifier(modifier, p);
        }

        Controller.PutCardToBattlefield(card);
      }
    }
  }
}