namespace Grove.Core.Modifiers
{
  using System;
  using System.Collections.Generic;
  using CardDsl;
  using Counters;
  using Infrastructure;
  using Preventions;
  using Redirections;

  [Copyable]
  public abstract class Modifier : IModifier, ICopyContributor
  {
    private TrackableList<Lifetime> _lifetimes;
    protected Game Game { get; private set; }
    protected ChangeTracker ChangeTracker { get { return Game.ChangeTracker; } }
    public Card Source { get; set; }
    public ITarget Target { get; private set; }
    protected Publisher Publisher { get { return Game.Publisher; } }
    protected int? X { get; private set; }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
      }
    }

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
    
    public void Dispose()
    {
      Unapply();

      Publisher.Unsubscribe(this);
      DisposeLifetimes();
    }

    public virtual void Activate()
    {
      Publisher.Subscribe(this);

      foreach (var lifetime in _lifetimes)
      {
        lifetime.Ended += RemoveModifier;
        Publisher.Subscribe(lifetime);  
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
        Publisher.Unsubscribe(lifetime);
        lifetime.Ended -= RemoveModifier;
        lifetime.Dispose();
      }

      _lifetimes.Clear();
    }

    protected virtual void Initialize() {}

    private void RemoveModifier(object sender, EventArgs e)
    {
      Target.RemoveModifier(this);
    }    

    [Copyable]
    public class Factory<TModifier> : IModifierFactory where TModifier : Modifier, new()
    {
      public bool EndOfTurn { get; set; }
      public int? MinLevel { get; set; }
      public int? MaxLevel { get; set; }
      public Game Game { get; set; }
      public Initializer<TModifier> Init = delegate { };

      public Modifier CreateModifier(Card modifierSource, ITarget modifierTarget, int? x = null)
      {
        var modifier = new TModifier();
        modifier._lifetimes = new TrackableList<Lifetime>(Game.ChangeTracker);
        modifier.Source = modifierSource;
        modifier.Target = modifierTarget;
        modifier.X = x;
        modifier.Game = Game;             

        foreach (var lifetime in CreateLifetimes(modifier))
        {
          modifier.AddLifetime(lifetime);
        }
        
        Init(modifier, new CardCreationContext(Game));                
        
        modifier.Initialize();        

        return modifier;
      }           

      private IEnumerable<Lifetime> CreateLifetimes(TModifier modifier)
      {
        yield return new PermanentLifetime(modifier, Game.ChangeTracker);

        if (EndOfTurn)
        {
          yield return new EndOfTurnLifetime(modifier, Game.ChangeTracker);          
        }

        if (modifier.Source.Is().Attachment)
        {
          yield return new AttachmentLifetime(modifier, Game.ChangeTracker);          
        }

        if (MinLevel.HasValue)
        {
          yield return new LevelLifetime(modifier, Game.ChangeTracker)
            {
              MinLevel = MinLevel.Value,
              MaxLevel = MaxLevel
            };
        }
      }
    }
  }
}