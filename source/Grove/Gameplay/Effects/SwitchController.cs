namespace Grove.Gameplay.Effects
{
  using System;
  using Modifiers;

  [Serializable]
  public class SwitchController : Effect
  {
    protected override void ResolveEffect()
    {
      var sourceModifier = new ChangeController(Controller.Opponent);
      sourceModifier.Initialize(new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          Target = Source.OwningCard,
          X = X
        }, Game);

      Source.OwningCard.AddModifier(sourceModifier);
    }
  }
}