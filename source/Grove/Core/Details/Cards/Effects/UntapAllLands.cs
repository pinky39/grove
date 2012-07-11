namespace Grove.Core.Details.Cards.Effects
{
  public class UntapAllLands : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var land in Controller.Battlefield.Lands)
      {
        land.Untap();
      }
    }
  }
}