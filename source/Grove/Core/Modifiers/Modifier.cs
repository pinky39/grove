namespace Grove.Core.Modifiers
{
  using System;
  using Counters;
  using Infrastructure;
  using Preventions;
  using Redirections;
  using Targeting;

  public delegate Modifier ModifierFactory(ModifierParameters p, Game game);

  [Copyable]
  public abstract class Modifier : GameObject, IModifier, ICopyContributor
  {
    private readonly TrackableList<Lifetime> _lifetimes = new TrackableList<Lifetime>();
    public Card Source { get; private set; }
    public ITarget Target { get; private set; }
    protected int? X { get; private set; }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
      }
    }

    public virtual void Apply(ControllerCharacteristic controller) {}
    public virtual void Apply(TriggeredAbilities abilities) {}
    public virtual void Apply(StaticAbilities abilities) {}
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

    public void AddLifetime(Lifetime lifetime)
    {
      lifetime.Initialize(Game);
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

    protected virtual void Initialize() {}

    private void RemoveModifier(object sender, EventArgs e)
    {
      Remove();
    }

    public void Remove()
    {
      Target.RemoveModifier(this);
    }

    public virtual void Initialize(ModifierParameters p, Game game)
    {
      Game = game;
      Source = p.Source;
      Target = p.Target;
      X = p.X;

      CreateLifetimes(p, game);
    }

    private void CreateLifetimes(ModifierParameters p, Game game)
    {
      _lifetimes.Add(new DefaultLifetime(p.Target));

      if (p.EndOfTurn)
      {
        _lifetimes.Add(new EndOfTurnLifetime());
      }

      if (p.Source.Is().Attachment)
      {
        _lifetimes.Add(new AttachmentLifetime(p.Source, p.Target.Card()));
      }

      if (p.MinLevel.HasValue)
      {
        _lifetimes.Add(new LevelLifetime(p.MinLevel.Value, p.MaxLevel, p.Target.Card()));
      }

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Initialize(game);
      }

      _lifetimes.Initialize(game.ChangeTracker);
    }
  }
}