namespace Grove
{
  using System.Collections.Generic;
  using Costs;
  using Infrastructure;
  using Modifiers;

  public delegate IModifier ModifierFactory();
  public delegate ICardModifier CardModifierFactory();
  public delegate IGameModifier GameModifierFactory();
  public delegate IPlayerModifier PlayerModifierFactory();

  [Copyable]
  public abstract class Modifier : GameObject, IModifier, ICopyContributor
  {
    public delegate bool CardSelector(Card card, Context ctx);

    private readonly List<Lifetime> _lifetimes = new List<Lifetime>();
    public bool UntilEot;
    public Card SourceCard { get; private set; }
    public Effect SourceEffect { get; private set; }
    public Card OwningCard { get { return (Card) Owner; } }
    public int? X { get; private set; }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
      }
    }

    public IModifiable Owner { get; private set; }

    public void Dispose()
    {
      Unapply();

      Unsubscribe(this);
      DisposeLifetimes();
    }

    public virtual void Activate()
    {
      Subscribe(this);

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
        Subscribe(lifetime);
      }
    }

    public void Initialize(ModifierParameters p, Game game)
    {
      Game = game;
      SourceCard = p.SourceCard;
      SourceEffect = p.SourceEffect;
      Owner = p.Owner;
      X = p.X;

      InitializeLifetimes(p.IsStatic);
      Initialize();
    }

    public void AddLifetime(Lifetime lifetime)
    {
      _lifetimes.Add(lifetime);
    }

    public virtual void Apply(CardController controller) {}
    public virtual void Apply(TriggeredAbilities abilities) {}
    public virtual void Apply(SimpleAbilities abilities) {}
    public virtual void Apply(ActivatedAbilities abilities) {}
    public virtual void Apply(ColorsOfCard colors) {}
    public virtual void Apply(DamagePreventions damagePreventions) {}
    public virtual void Apply(Protections protections) {}
    public virtual void Apply(TypeOfCard typeOfCard) {}
    public virtual void Apply(Counters counters) {}
    public virtual void Apply(Level level) {}
    public virtual void Apply(DamageRedirections damageRedirections) {}
    public virtual void Apply(ContiniousEffects continiousEffects) {}
    public virtual void Apply(LandLimit landLimit) {}
    public virtual void Apply(Strenght strenght) {}
    public virtual void Apply(SkipSteps skipSteps) {}
    public virtual void Apply(CostModifiers costModifiers) {}
    public virtual void Apply(MinimumBlockerCount count) {}
    public virtual void Apply(CardBase cardBase) {}
    public virtual void Apply(StaticAbilities abilities) {}
    public virtual void Apply(NamedGameModifiers namedGameModifiers) {}
    public virtual void Apply(CombatCost combatCost) {}

    protected abstract void Unapply();

    private void DisposeLifetimes()
    {
      foreach (var lifetime in _lifetimes)
      {
        Unsubscribe(lifetime);
        lifetime.Ended -= RemoveModifier;
      }
    }

    private void RemoveModifier()
    {
      Remove();
    }

    private void Remove()
    {
      Owner.RemoveModifier(this);
    }

    protected virtual void Initialize() {}

    private void InitializeLifetimes(bool isStatic)
    {
      if (isStatic == false)
      {
        if (Owner is Card)
        {
          // if modifier is applied to a card in 
          // the battlefield it should be removed 
          // when the card it is aplied to leaves 
          // the battlefield
          _lifetimes.Add(new OwningCardLifetime());
        }

        if (UntilEot)
        {
          _lifetimes.Add(new EndOfTurnLifetime());
        }

        if (SourceCard.Is().Attachment &&
          /* when attachment applies modifier to itself e.g Avarice Amulet this should not apply */
            SourceCard != Owner)
        {                              
          // when modifier is given by an attachment
          // it should become invalid when attachment
          // becomes detached
          _lifetimes.Add(new AttachmentLifetime());
        }
      }

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Initialize(Game, this);
      }
    }

    protected Context Ctx { get { return new Context(this, Game); } }

    public class Context
    {
      private readonly Modifier _modifier;
      private readonly Game _game;

      public Context(Modifier modifier, Game game)
      {
        _modifier = modifier;
        _game = game;
      }

      public Card OwningCard { get { return _modifier.OwningCard; } }
      public Players Players { get { return _game.Players; } }
    }
  }
}