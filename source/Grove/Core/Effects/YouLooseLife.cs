namespace Grove.Effects
{
  public class YouLooseLife : Effect
  {
    private readonly int _amount;
    private Player _you;

    private YouLooseLife() {}

    public YouLooseLife(int amount)
    {
      _amount = amount;
    }

    protected override void Initialize()
    {
      _you = Source.OwningCard.Controller;
    }

    protected override void ResolveEffect()
    {
      _you.Life -= _amount;
    }
  }
}