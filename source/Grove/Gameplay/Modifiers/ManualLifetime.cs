namespace Grove.Gameplay.Modifiers
{
  public class ManualLifetime : Lifetime
  {
    public void EndLife()
    {
      End();
    }
  }
}