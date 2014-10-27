namespace Grove.Effects
{
  public class ChangeLifeOfEnchantedPermanentsController : Effect
  {
    private readonly DynParam<int> _amount;
    private Player _player;

    private ChangeLifeOfEnchantedPermanentsController() {}

    public ChangeLifeOfEnchantedPermanentsController(DynParam<int> amount)
    {
      _amount = amount;

      RegisterDynamicParameters(amount);
    }

    protected override void Initialize()
    {
      _player = Source.OwningCard.AttachedTo.Controller;
    }

    protected override void ResolveEffect()
    {
      _player.Life += _amount.Value;
    }
  }
}