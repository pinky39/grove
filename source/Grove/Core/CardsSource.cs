namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Costs;
  using Counters;
  using DamagePrevention;
  using Effects;
  using Modifiers;
  using Triggers;
  using Zones;

  public delegate void Initializer<in T>(T target, Creator creationContext);

  public class Creator
  {
    private readonly Game _game;

    public Creator(Game game)
    {
      _game = game;
    }

    public Card.CardFactory Card
    {
      get { return new Card.CardFactory(_game); }
    }

    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      IEffectFactory effect,
      ITargetSelectorFactory selector = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      Func<Game, Card, ActivationParameters, bool> timing = null)
    {
      return new ActivatedAbility.Factory<ActivatedAbility>
        {
          Game = _game,
          Init = self =>
            {
              self.Text = text;
              self.EffectCategories = category;
              self.SetCost(cost);
              self.Timing(timing);
              self.Effect(effect);
              self.ActivateOnlyAsSorcery = activateAsSorcery;

              if (selector != null)
                self.SetTargetSelector(selector);
            }
        };
    }

    public ContinuousEffect.Factory Continuous(Initializer<ContinuousEffect> init)
    {
      return new ContinuousEffect.Factory
        {
          Game = _game,
          Init = init,
        };
    }

    public ICostFactory Cost<T>(Initializer<T> init = null) where T : Cost, new()
    {
      init = init ?? delegate { };
      return new Cost.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public ICounterFactory Counter<T>(Initializer<T> init = null) where T : Counter, new()
    {
      init = init ?? delegate { };

      return new Counter.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public IEffectFactory Effect<T>(Initializer<T> init = null) where T : Effect, new()
    {
      init = init ?? delegate { };

      return new Effect.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public IActivatedAbilityFactory ManaAbility(Mana mana, string text, ICostFactory costFactory = null,
                                                int? priority = null)
    {
      costFactory = costFactory ?? new Cost.Factory<TapOwnerPayMana>
        {
          Game = _game,
          Init = (cost, _) => { cost.TapOwner = true; }
        };

      return new ActivatedAbility.Factory<ManaAbility>
        {
          Game = _game,
          Init = ability =>
            {
              ability.SetManaAmount(mana.ToAmount());
              ability.Text = text;
              ability.SetCost(costFactory);
              ability.Priority = priority ?? DefaultManaSourcePriority(ability);
            }
        };
    }

    public IModifierFactory Modifier<T>(Initializer<T> init = null, bool untilEndOfTurn = false, int? minLevel = null,
                                        int? maxLevel = null) where T : Modifier, new()
    {
      init = init ?? delegate { };

      return new Modifier.Factory<T>
        {
          Game = _game,
          Init = init,
          EndOfTurn = untilEndOfTurn,
          MinLevel = minLevel,
          MaxLevel = maxLevel
        };
    }

    public IDamagePreventionFactory Prevention<T>(Initializer<T> init = null)
      where T : DamagePrevention.DamagePrevention, new()
    {
      init = init ?? delegate { };

      return new DamagePrevention.DamagePrevention.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public ITargetSelectorFactory Selector(TargetValidator validator, ScoreCalculator scorer = null,
                                           Zone zone = Zone.Battlefield)
    {
      scorer = scorer ?? delegate { return WellKnownTargetScores.Good; };

      return new TargetSelector.Factory
        {
          Game = _game,
          Init = selector =>
            {
              selector.Validator = (target, source, game) =>
                {
                  if (target.IsCard() && target.Card().Zone != zone)
                  {
                    return false;
                  }
                  return validator(target, source, game);
                };
              selector.Scorer = scorer;
            }
        };
    }

    public ITargetSelectorFactory Selector(Func<ITarget, bool> validator, ScoreCalculator scorer = null,
                                           Zone zone = Zone.Battlefield)
    {
      return Selector((target, source, game) => validator(target), scorer, zone);
    }

    public ITargetSelectorFactory Selector(Func<ITarget, Card, bool> validator, ScoreCalculator scorer = null,
                                           Zone zone = Zone.Battlefield)
    {
      return Selector((target, source, game) => validator(target, source), scorer, zone);
    }

    public TriggeredAbility.Factory StaticAbility(
      ITriggerFactory trigger,
      IEffectFactory effect
      )
    {
      // this is not a real triggered ability, 
      // but a static ability which activates
      // with trigger and does not use the stack
      return new TriggeredAbility.Factory
        {
          Game = _game,
          Init = self =>
            {
              self.AddTrigger(trigger);
              self.Effect(effect);
              self.UsesStack = false;
            }
        };
    }

    public ITriggerFactory Trigger<T>(Initializer<T> init = null) where T : Trigger, new()
    {
      init = init ?? delegate { };

      return new Trigger.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public TriggeredAbility.Factory TriggeredAbility(
      string text,
      IEnumerable<ITriggerFactory> triggers,
      IEffectFactory effect,
      ITargetSelectorFactory targetSelector = null,
      EffectCategories category = EffectCategories.Generic)
    {
      return new TriggeredAbility.Factory
        {
          Game = _game,
          Init = self =>
            {
              self.Text = text;

              foreach (var triggerFactory in triggers)
              {
                self.AddTrigger(triggerFactory);
              }

              self.Effect(effect);
              self.EffectCategories = category;
              if (targetSelector != null)
                self.SetTargetSelector(targetSelector);
            }
        };
    }

    public TriggeredAbility.Factory TriggeredAbility(
      string text,
      ITriggerFactory trigger,
      IEffectFactory effect,
      ITargetSelectorFactory targetSelector = null,
      EffectCategories category = EffectCategories.Generic)
    {
      return new TriggeredAbility.Factory
        {
          Game = _game,
          Init = self =>
            {
              self.Text = text;
              self.AddTrigger(trigger);
              self.Effect(effect);
              self.EffectCategories = category;
              if (targetSelector != null)
                self.SetTargetSelector(targetSelector);
            }
        };
    }

    private static int DefaultManaSourcePriority(ManaAbility ability)
    {
      return ability.OwningCard.Is().Land ? ManaSourcePriorities.Land : ManaSourcePriorities.Creature;
    }
  }

  public abstract class CardsSource
  {
    public Creator C
    {
      get { return new Creator(Game); }
    }

    public Game Game { get; set; }

    public abstract IEnumerable<ICardFactory> GetCards();

    protected Func<Game, Card, ActivationParameters, bool> All(
      params Func<Game, Card, ActivationParameters, bool>[] predicates)
    {
      return
        (game, card, activationParameters) => predicates.All(predicate => predicate(game, card, activationParameters));
    }

    protected Func<Game, Card, ActivationParameters, bool> Any(
      params Func<Game, Card, ActivationParameters, bool>[] predicates)
    {
      return
        (game, card, activationParameters) => predicates.Any(predicate => predicate(game, card, activationParameters));
    }

    protected T[] L<T>(params T[] elt)
    {
      return elt;
    }

    public LevelDefinition Level(int min, int power, int toughness, StaticAbility ability, int? max = null)
    {
      return new LevelDefinition
        {
          Min = min,
          Max = max,
          Power = power,
          Thoughness = toughness,
          StaticAbility = ability
        };
    }

    public Card.CardFactory Leveler(Card.CardFactory card, IManaAmount cost, params LevelDefinition[] levelDefinitions)
    {
      var abilities = new List<object>();

      abilities.Add(
        C.ActivatedAbility(
          String.Format("{0}: Put a level counter on this. Level up only as sorcery.", cost),
          C.Cost<TapOwnerPayMana>((c, _) => c.Amount = cost),
          C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(c.Modifier<IncreaseLevel>())),
          timing: Timings.Steps(Step.FirstMain), activateAsSorcery: true));


      foreach (var levelDefinition in levelDefinitions)
      {
        var definition = levelDefinition;

        abilities.Add(
          C.StaticAbility(
            C.Trigger<OnLevelChanged>((c, _) => c.Level = definition.Min),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = definition.StaticAbility,
                minLevel: definition.Min, maxLevel: definition.Max),
              c.Modifier<SetPowerAndToughness>((m, _) =>
                {
                  m.Power = definition.Power;
                  m.Tougness = definition.Thoughness;
                }, minLevel: definition.Min, maxLevel: definition.Max))))
          );
      }

      card.IsLeveler().Abilities(abilities.ToArray());

      return card;
    }

    public class LevelDefinition
    {
      public int Min { get; set; }
      public int? Max { get; set; }

      public int Power { get; set; }
      public int Thoughness { get; set; }

      public StaticAbility StaticAbility { get; set; }
    }
  }
}