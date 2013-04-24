namespace Grove.Core.Effects
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