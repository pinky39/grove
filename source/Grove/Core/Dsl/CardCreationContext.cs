namespace Grove.Core.Dsl
{
  using System.Collections.Generic;
  using Ai;
  using Details.Cards;
  using Details.Cards.Costs;
  using Details.Cards.Counters;
  using Details.Cards.Effects;
  using Details.Cards.Modifiers;
  using Details.Cards.Preventions;
  using Details.Cards.Redirections;
  using Details.Cards.Triggers;
  using Details.Mana;
  using Targeting;

  public class CardCreationContext
  {
    private readonly Game _game;

    public CardCreationContext(Game game)
    {
      _game = game;
    }

    public Card.CardFactory Card { get { return new Card.CardFactory(_game); } }


    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      IEffectFactory effect,
      ITargetSelectorFactory effectSelector = null,
      ITargetSelectorFactory costSelector = null,
      TargetsFilterDelegate targetFilter = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      TimingDelegate timing = null)
    {
      var effectSelectors = effectSelector == null
        ? new ITargetSelectorFactory[] {}
        : new[] {effectSelector};

      return ActivatedAbility(text, cost, effect, effectSelectors, costSelector,
        targetFilter, activateAsSorcery, category, timing);
    }

    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      IEffectFactory effect,
      ITargetSelectorFactory[] effectSelectors,
      ITargetSelectorFactory costSelector = null,
      TargetsFilterDelegate targetFilter = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      TimingDelegate timing = null)
    {
      return new ActivatedAbility.Factory<ActivatedAbility>
        {
          Game = _game,
          Init = self =>
            {
              self.Text = text;
              self.EffectCategories = category;              
              self.Timing(timing);
              self.Effect(effect);
              self.ActivateOnlyAsSorcery = activateAsSorcery;

              if (effectSelectors.Length > 0)
              {
                self.EffectTargets(effectSelectors);
              }

              if (costSelector != null)
                self.CostTargets(costSelector);

              self.SetCost(cost);

              if (targetFilter != null)
                self.TargetsFilter(targetFilter);
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

    public IActivatedAbilityFactory ManaAbility(ManaUnit mana, string text, ICostFactory cost = null,
                                                int? priority = null)
    {
      return ManaAbility(mana.ToAmount(), text, cost, priority);
    }

    public IActivatedAbilityFactory ManaAbility(IManaAmount manaAmount, string text, ICostFactory cost = null,
                                                int? priority = null)
    {
      cost = cost ?? new Cost.Factory<TapOwnerPayMana>
        {
          Game = _game,
          Init = (cst, _) => { cst.TapOwner = true; }
        };

      return new ActivatedAbility.Factory<ManaAbility>
        {
          Game = _game,
          Init = ability =>
            {
              ability.SetManaAmount(manaAmount);
              ability.Text = text;
              ability.SetCost(cost);
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

    public IDamageRedirectionFactory Redirection<T>(Initializer<T> init = null) where T : DamageRedirection, new()
    {
      init = init ?? delegate { };

      return new DamageRedirection.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public IDamagePreventionFactory Prevention<T>(Initializer<T> init = null)
      where T : DamagePrevention, new()
    {
      init = init ?? delegate { };

      return new DamagePrevention.Factory<T>
        {
          Game = _game,
          Init = init
        };
    }

    public ITargetSelectorFactory Selector(TargetValidatorDelegate validator, string text = null)
    {
      return new TargetSelector.Factory
        {
          Game = _game,
          Init = selector =>
            {
              selector.Validator = validator;

              if (text != null)
                selector.TextFormat = text;
            }
        };
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
      EffectCategories category = EffectCategories.Generic,
      bool triggerOnlyIfOwningCardIsInPlay = false)
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
              self.TriggerOnlyIfOwningCardIsInPlay = triggerOnlyIfOwningCardIsInPlay;
              if (targetSelector != null)
                self.EffectTargets(targetSelector);
            }
        };
    }

    public TriggeredAbility.Factory TriggeredAbility(
      string text,
      ITriggerFactory trigger,
      IEffectFactory effect,
      ITargetSelectorFactory targetSelector = null,
      TargetsFilterDelegate targetFilter = null,
      EffectCategories category = EffectCategories.Generic,
      bool triggerOnlyIfOwningCardIsInPlay = false)
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
              self.TriggerOnlyIfOwningCardIsInPlay = triggerOnlyIfOwningCardIsInPlay;

              if (targetSelector != null)
                self.EffectTargets(targetSelector);

              if (targetFilter != null)
                self.TargetsFilter(targetFilter);
            }
        };
    }

    private static int DefaultManaSourcePriority(ManaAbility ability)
    {
      return ability.OwningCard.Is().Land ? ManaSourcePriorities.Land : ManaSourcePriorities.Creature;
    }
  }
}