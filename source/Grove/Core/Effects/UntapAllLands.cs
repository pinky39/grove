namespace Grove.Core.Effects
{
  public class UntapAllLands : Effect
  {
    public override void Resolve()
    {
      foreach (var land in Controller.Battlefield.Lands)
      {
        land.Untap();
      }
    }
  }
}