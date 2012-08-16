namespace Grove.Core.Details.Cards
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Zones;

  public class ControllerCharacteristic : Characteristic<Player>, IModifiable
  {
    private readonly Card _card;
    private readonly Publisher _publisher;

    private ControllerCharacteristic() {}

    public ControllerCharacteristic(Player value, Card card, Publisher publisher, ChangeTracker changeTracker,
      IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy)
    {
      _card = card;
      _publisher = publisher;
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

        _publisher.Publish(new ControllerChanged(_card));
      }
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}