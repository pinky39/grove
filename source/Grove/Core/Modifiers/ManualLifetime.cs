namespace Grove.Modifiers
{
  public class ManualLifetime : Lifetime
  {
    public void EndLife()
    {
      End();
    }
  }
}