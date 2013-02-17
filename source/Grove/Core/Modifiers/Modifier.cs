namespace Grove.Core.Modifiers
{
  using System;
  using Counters;
  using Effects;
  using Infrastructure;
  using Preventions;
  using Redirections;
  using Targeting;

  public delegate Modifier ModifierFactory();

  [Copyable]
  public abstract class Modifier : GameObject, IModifier, ICopyContributor
  {
    private readonly TrackableList<Lifetime> _lifetimes = new TrackableList<Lifetime>();
    public Card Source { get; private set; }
    public Effect SourceEffect { get; private set; }
    public ITarget Target { get; private set; }
    public int? X { get; private set; }
    public bool UntilEot;    

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

    public void Remove()
    {
      Target.RemoveModifier(this);
    }

    public virtual Modifier Initialize(ModifierParameters p, Game game)
    {
      Game = game;
      Source = p.SourceCard;
      Target = p.Target;
      SourceEffect = p.SourceEffect;
      X = p.X;

      CreateDefaultLifetimes();

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Initialize(this, game);
      }

      _lifetimes.Initialize(game.ChangeTracker);

      return this;
    }

    private void CreateDefaultLifetimes()
    {
      _lifetimes.Add(new DefaultLifetime());

      if (UntilEot)
      {
        _lifetimes.Add(new EndOfTurnLifetime());
      }

      if (Source.Is().Attachment)
      {
        _lifetimes.Add(new AttachmentLifetime());
      }      
    }
  }
}