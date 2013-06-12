namespace Grove.Gameplay.Modifiers
{
  using System;
  using Abilities;
  using Characteristics;
  using Counters;
  using DamageHandling;
  using Effects;
  using Infrastructure;
  using Misc;

  public delegate IModifier ModifierFactory();
  public delegate ICardModifier CardModifierFactory();
  public delegate IGameModifier GameModifierFactory();
  public delegate IPlayerModifier PlayerModifierFactory();

  [Copyable]
  public abstract class Modifier : GameObject, IModifier, ICopyContributor
  {
    private readonly TrackableList<Lifetime> _lifetimes = new TrackableList<Lifetime>();
    public bool UntilEot;
    public Card SourceCard { get; private set; }
    public Effect SourceEffect { get; private set; }
    public Card OwningCard { get { return Owner.As<Card>(); } }
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

    public virtual void Apply(ControllerCharacteristic controller) {}
    public virtual void Apply(TriggeredAbilities abilities) {}
    public virtual void Apply(SimpleAbilities abilities) {}
    public virtual void Apply(ActivatedAbilities abilities) {}
    public virtual void Apply(CardColors colors) {}
    public virtual void Apply(Power power) {}
    public virtual void Apply(Toughness toughness) {}
    public virtual void Apply(DamagePreventions damagePreventions) {}
    public virtual void Apply(Protections protections) {}
    public virtual void Apply(CardTypeCharacteristic cardType) {}
    public virtual void Apply(Counters counters) {}
    public virtual void Apply(Level level) {}
    public virtual void Apply(DamageRedirections damageRedirections) {}
    public virtual void Apply(ContiniousEffects continiousEffects) {}
    public virtual void Apply(LandLimit landLimit) {}

    public void AddLifetime(Lifetime lifetime)
    {
      _lifetimes.Add(lifetime);
    }

    protected abstract void Unapply();

    private void DisposeLifetimes()
    {
      foreach (var lifetime in _lifetimes)
      {
        Unsubscribe(lifetime);
        lifetime.Ended -= RemoveModifier;
        lifetime.Dispose();
      }

      _lifetimes.Clear();
    }

    private void RemoveModifier(object sender, EventArgs e)
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
      _lifetimes.Initialize(ChangeTracker);

      if (isStatic == false)
      {
        if (Owner is Card)
        {
          _lifetimes.Add(new OwningCardLifetime());
        }

        if (UntilEot)
        {
          _lifetimes.Add(new EndOfTurnLifetime());
        }

        if (SourceCard.Is().Attachment)
        {
          _lifetimes.Add(new AttachmentLifetime());
        }
      }

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Initialize(Game, this);
      }
    }
  }
}