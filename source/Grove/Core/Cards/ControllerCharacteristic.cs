namespace Grove.Core.Cards
{
  using System.Linq;
  using Grove.Infrastructure;
  using Grove.Core.Messages;
  using Grove.Core.Zones;
  using Modifiers;

  public class ControllerCharacteristic : Characteristic<Player>, IModifiable
  {
    private readonly Card _card;
    private readonly Game _game;

    private ControllerCharacteristic() {}

    public ControllerCharacteristic(Player value, Card card, Game game,
      IHashDependancy hashDependancy)
      : base(value, game.ChangeTracker, hashDependancy)
    {
      _card = card;
      _game = game;
    }

    public override Player Value
    {
      get { return base.Value; }
      protected set
      {
        // no change
        if (value == base.Value)
          return;

        base.Value = value;

        if (_card.Zone != Zone.Battlefield)
          return;                
        
        if (!_card.IsAttached)
        {
          value.PutCardToBattlefield(_card);

          foreach (var attachment in _card.Attachments.Where(x => x.Is().Aura || x.Is().Equipment))
          {
            // for auras and equipments just change battlefield
            // do not change the control          
            value.PutCardToBattlefield(attachment);
          }
        }

        _game.Publish(new ControllerChanged(_card));
      }
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}