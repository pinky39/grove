namespace Grove.Core.Cards.Modifiers
{
  using System;
  using System.Collections.Generic;
  using Counters;
  using Dsl;
  using Infrastructure;
  using Preventions;
  using Redirections;
  using Targeting;

  [Copyable]
  public abstract class Modifier : IModifier, ICopyContributor
  {
    private TrackableList<Lifetime> _lifetimes;
    protected Game Game { get; private set; }
    protected ChangeTracker ChangeTracker { get { return Game.ChangeTracker; } }
    public Card Source { get; set; }
    public ITarget Target { get; private set; }
    protected int? X { get; private set; }
    protected CardBuilder Builder { get { return new CardBuilder(); } }

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

    public void Dispose()
    {
      Unapply();

      Game.Unsubscribe(this);
      DisposeLifetimes();
    }

    public virtual void Activate()
    {
      Game.Subscribe(this);

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
        Game.Subscribe(lifetime);
      }
    }

    public void AddLifetime(ILifetimeFactory lifetimeFactory)
    {
      _lifetimes.Add(lifetimeFactory.CreateLifetime(Game));
    }

    protected abstract void Unapply();

    private void DisposeLifetimes()
    {
      foreach (var lifetime in _lifetimes)
      {
        Game.Unsubscribe(lifetime);
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

    [Copyable]
    public class Factory<TModifier> : IModifierFactory where TModifier : Modifier, new()
    {
      public Initializer<TModifier> Init = delegate { };
      public bool EndOfTurn { get; set; }
      public int? MinLevel { get; set; }
      public int? MaxLevel { get; set; }

      public Modifier CreateModifier(Card source, ITarget target, int? x, Game game)
      {
        var modifier = new TModifier();
        modifier._lifetimes = new TrackableList<Lifetime>(game.ChangeTracker);
        modifier.Source = source;
        modifier.Target = target;
        modifier.X = x;
        modifier.Game = game;

        foreach (var lifetimeFactory in CreateLifetimes(modifier))
        {
          modifier.AddLifetime(lifetimeFactory);
        }

        Init(modifier);
        modifier.Initialize();

        return modifier;
      }

      private IEnumerable<ILifetimeFactory> CreateLifetimes(TModifier modifier)
      {
        var builder = new CardBuilder();

        yield return builder.Lifetime<DefaultLifetime>(l => l.Target = modifier.Target);

        if (EndOfTurn)
        {
          yield return builder.Lifetime<EndOfTurnLifetime>();
        }

        if (modifier.Source.Is().Attachment)
        {
          yield return builder.Lifetime<AttachmentLifetime>(l =>
            {
              l.Attachment = modifier.Source;
              l.AttachmentTarget = modifier.Target.Card();
            });
        }

        if (MinLevel.HasValue)
        {
          yield return builder.Lifetime<LevelLifetime>(l =>
            {
              l.MinLevel = MinLevel.Value;
              l.MaxLevel = MaxLevel;
              l.ModifierTarget = modifier.Target.Card();
            });
        }
      }
    }
  }
}