namespace Grove.Gameplay.Characteristics
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Zones;

  public class ControllerCharacteristic : Characteristic<Player>, IModifiable
  {
    private Card _card;

    private ControllerCharacteristic() {}

    public ControllerCharacteristic(Player controller) : base(controller) {}

    public void Accept(IModifier modifier)
    {
      var before = Value;

      modifier.Apply(this);

      var after = Value;

      if (before == after)
        return;

      if (_card.Zone != Zone.Battlefield)
        return;

      Combat.Remove(_card);

      if (!_card.IsAttached)
      {
        after.PutCardToBattlefield(_card);

        foreach (var attachment in _card.Attachments.Where(x => x.Is().Aura || x.Is().Equipment))
        {
          // for auras and equipments just change battlefield
          // do not change the control          
          after.PutCardToBattlefield(attachment);
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