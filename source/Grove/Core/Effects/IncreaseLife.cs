namespace Grove.Core.Effects
{
  public class IncreaseLife : Effect
  {
    public int Amount { get; set; }

    public override void Resolve()
    {
      Controller.Life += Amount;
    }    
  }
}