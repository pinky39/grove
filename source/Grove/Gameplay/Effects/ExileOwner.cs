namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;
  using Zones;

  public class ExileOwner : Effect
  {
    private Zone _owningCardZone;

    protected override void Initialize()
    {
      _owningCardZone = Source.OwningCard.Zone;
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.ExileFrom(_owningCardZone);
    }
  }
}