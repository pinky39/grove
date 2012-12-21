namespace Grove.Ui
{
  using Core.Mana;
  using Core.Messages;
  using Infrastructure;

  public class CardViewModel : IReceive<StateChanged>
  {
    public CardViewModel(Core.Card card)
    {
      Card = card;
    }

    public Core.Card Card { get; private set; }

    public virtual int? Power { get; protected set; }
    public virtual int? Toughness { get; protected set; }
    public virtual bool IsFaceUp { get; protected set; }
    public virtual ManaColors Colors { get; protected set; }
    public virtual int? Counters { get; protected set; }
    public virtual int? Level { get; protected set; }
    public virtual string Type { get; protected set; }
    public virtual int Damage { get; protected set; }
    public virtual bool IsTapped { get; protected set; }

    public void Receive(StateChanged message)
    {
      Update();
    }

    private void Update()
    {
      Power = Card.Power;
      Toughness = Card.Toughness;
      IsFaceUp = Card.IsVisibleInUi;
      Colors = Card.Colors;
      Counters = Card.Counters;
      Level = Card.Level;
      Type = Card.Type;
      Damage = Card.Damage;
      IsTapped = Card.IsTapped;
    }
  }
}