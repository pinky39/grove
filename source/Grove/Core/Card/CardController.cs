namespace Grove
{
  using System.Linq;
  using Events;
  using Grove.Infrastructure;
  using Modifiers;

  public class CardController : Characteristic<Player>, IAcceptsCardModifier
  {
    private Card _card;

    private CardController() { }

    public CardController(Player controller) : base(controller) { }

    public void Accept(ICardModifier modifier) { modifier.Apply(this); }

    protected override void OnCharacteristicChanged(Player oldValue, Player newValue)
    {
      if (_card.Zone != Zone.Battlefield)
        return;

      Combat.Remove(_card);

      if (!_card.IsAttached)
      {     
        foreach (var attachment in _card.Attachments.Where(x => x.IsPermanent && (x.Is().Aura || x.Is().Equipment)).ToList())
        {
          _card.Detach(attachment);
        }

        Value.PutCardToBattlefield(_card);
      }

      _card.HasSummoningSickness = true;

      Publish(new ControllerChangedEvent(_card));
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _card = (Card) hashDependancy;
    }
  }
}