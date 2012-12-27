namespace Grove.Core.Cards
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Zones;

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

    public void Accept(IModifier modifier)
    {
      var before = this.Value;

      modifier.Apply(this);

      var after = this.Value;

      if (before == after)
        return;

      if (_card.Zone != Zone.Battlefield)
        return;
      
      _game.Combat.Remove(_card);

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

      _game.Publish(new ControllerChanged(_card));
    }
  }
}