namespace Grove.Core.Controllers
{
  using Effects;
  using Zones;

  public delegate void PayLifeOrManaHandler(PayLifeOrManaArguments args);

  public class PayLifeOrManaArguments
  {
    public bool Answer { get; set; }
    public Effect Effect { get; set; }    
    public Player Player { get; set; }
    public Stack Stack { get; set; }
  }
}