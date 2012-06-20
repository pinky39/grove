namespace Grove.Core.Effects
{
  public class IncreaseLife : Effect
  {
    public int Amount { get; set; }

    protected override void ResolveEffect()
    {
      Controller.Life += Amount;
    }    
  }
}