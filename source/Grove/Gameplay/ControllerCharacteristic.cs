namespace Grove.Gameplay
{
  using System.Linq;
  using Grove.Infrastructure;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Modifiers;

  public class ControllerCharacteristic : Characteristic<Player>, IAcceptsCardModifier
  {
    private Card _card;

    private ControllerCharacteristic() {}

    public ControllerCharacteristic(Player controller) : base(controller) {}

    public void Accept(ICardModifier modifier)
    {      
      modifier.Apply(this);
    }

    protected override void OnCharacteristicChanged(Player oldValue, Player newValue)
    {
      if (_card.Zone != Zone.Battlefield)
        return;
      
      Combat.Remove(_card);

      if (!_card.IsAttached)
      {
        Value.PutCardToBattlefield(_card);

        foreach (var attachment in _card.Attachments.Where(x => x.IsPermanent && (x.Is().Aura || x.Is().Equipment)))
        {
          // for auras and equipments just change battlefield
          // do not change the control          
          Value.PutCardToBattlefield(attachment);
        }
      }

      _card.HasSummoningSickness = true;

      Publish(new ControllerChanged(_card));
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _card = (Card) hashDependancy;
    }
  }
}