namespace Grove.Core.Dsl
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Cards;
  using Cards.Costs;
  using Cards.Counters;
  using Cards.Effects;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Cards.Redirections;
  using Cards.Triggers;
  using Mana;
  using Targeting;
  using Zones;

  public class CardBuilder
  {
    public CardFactory Card { get { return new CardFactory(); } }

    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      IEffectFactory effect,
      ITargetValidatorFactory effectValidator = null,
      ITargetValidatorFactory costValidator = null,
      TargetSelectorAiDelegate targetSelectorAi = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      TimingDelegate timing = null,
      Zone activationZone = Zone.Battlefield)
    {
      var effectSelectors = effectValidator == null
        ? new ITargetValidatorFactory[] {}
        : new[] {effectValidator};

      return ActivatedAbility(text, cost, effect, effectSelectors, costValidator,
        targetSelectorAi, activateAsSorcery, category, timing, activationZone);
    }

    public ILifetimeFactory Lifetime<T>(Initializer<T> init = null) where T : Lifetime, new()
    {
      init = init ?? delegate { };

      return new Lifetime.Factory<T>
        {
          Init = init
        };
    }

    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      IEffectFactory effect,
      ITargetValidatorFactory[] effectValidators,
      ITargetValidatorFactory costValidator = null,
      TargetSelectorAiDelegate targetSelectorAi = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      TimingDelegate timing = null,
      Zone activationZone = Zone.Battlefield)
    {
      return new ActivatedAbility.Factory<ActivatedAbility>
        {
          Init = self =>
            {
              self.Text = text;
              self.EffectCategories = category;
              self.Timing(timing);
              self.Effect(effect);
              self.ActivateOnlyAsSorcery = activateAsSorcery;
              self.ActivationZone = activationZone;

              var costValidators = new List<ITargetValidatorFactory>();

              if (costValidator != null)
                costValidators.Add(costValidator);

              self.Targets(effectValidators, costValidators, targetSelectorAi);

              self.SetCost(cost);
            }
        };
    }

    public ContinuousEffect.Factory Continuous(Initializer<ContinuousEffect> init)
    {
      return new ContinuousEffect.Factory
        {
          Init = init,
        };
    }

    public ICostFactory Cost<T>(Initializer<T> init = null) where T : Cost, new()
    {
      init = init ?? delegate { };
      return new Cost.Factory<T>
        {
          Init = init
        };
    }

    public ICounterFactory Counter<T>(Initializer<T> init = null) where T : Counter, new()
    {
      init = init ?? delegate { };

      return new Counter.Factory<T>
        {
          Init = init
        };
    }

    public IEffectFactory Effect<T>(Action<T> init = null) where T : Effect, new()
    {
      init = init ?? delegate { };

      return Effect<T>(p => init(p.Effect));
    }

    public IEffectFactory Effect<T>(EffectInitializer<T> init) where T : Effect, new()
    {
      init = init ?? delegate { };

      return new Effect.Factory<T>
        {
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
          Init = cst => { cst.TapOwner = true; }
        };

      return new ActivatedAbility.Factory<ManaAbility>
        {
          Init = ability =>
            {
              ability.SetManaAmount(manaAmount);
              ability.Text = text;
              ability.SetCost(cost);
              ability.Priority = priority ?? DefaultManaSourcePriority(ability);
            }
        };
    }

    public IModifierFactory Modifier<T>(Initializer<T> init = null, bool untilEndOfTurn = false,
      int? minLevel = null,
      int? maxLevel = null) where T : Modifier, new()
    {
      init = init ?? delegate { };

      return new Modifier.Factory<T>
        {
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
          Init = init
        };
    }

    public IDamagePreventionFactory Prevention<T>(Initializer<T> init = null)
      where T : DamagePrevention, new()
    {
      init = init ?? delegate { };

      return new DamagePrevention.Factory<T>
        {
          Init = init
        };
    }

    public ITargetValidatorFactory TargetValidator(TargetValidatorDelegate target, ZoneValidatorDelegate zone,
      string text = null,
      bool mustBeTargetable = true, int minCount = 1, int maxCount = 1)
    {
      return new TargetValidator.Factory
        {
          Init = selector =>
            {
              selector.Target = target;
              selector.Zone = zone;

              selector.MustBeTargetable = mustBeTargetable;
              selector.MinCount = minCount;
              selector.MaxCount = maxCount;

              if (text != null)
                selector.MessageFormat = text;
            }
        };
    }

    public TriggeredAbility.Factory StaticAbility(
      ITriggerFactory trigger,
      IEffectFactory effect,
      bool triggerOnlyIfOwningCardIsInPlay = false
      )
    {
      // this is not a real triggered ability, 
      // but a static ability which activates
      // with trigger and does not use the stack
      return new TriggeredAbility.Factory
        {
          Init = self =>
            {
              self.AddTrigger(trigger);
              self.Effect(effect);
              self.UsesStack = false;
              self.TriggerOnlyIfOwningCardIsInPlay = triggerOnlyIfOwningCardIsInPlay;
            }
        };
    }

    public ITriggerFactory Trigger<T>(Initializer<T> init = null) where T : Trigger, new()
    {
      init = init ?? delegate { };

      return new Trigger.Factory<T>
        {
          Init = init
        };
    }

    public TriggeredAbility.Factory TriggeredAbility(
      string text,
      IEnumerable<ITriggerFactory> triggers,
      IEffectFactory effect,
      EffectCategories category = EffectCategories.Generic,
      bool triggerOnlyIfOwningCardIsInPlay = false)
    {
      return new TriggeredAbility.Factory
        {
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
            }
        };
    }

    public TriggeredAbility.Factory TriggeredAbility(
      string text,
      ITriggerFactory trigger,
      IEffectFactory effect,
      ITargetValidatorFactory effectValidator = null,
      TargetSelectorAiDelegate selectorAi = null,
      EffectCategories abilityCategory = EffectCategories.Generic,
      bool triggerOnlyIfOwningCardIsInPlay = false)
    {
      return new TriggeredAbility.Factory
        {
          Init = self =>
            {
              self.Text = text;
              self.AddTrigger(trigger);
              self.Effect(effect);
              self.EffectCategories = abilityCategory;
              self.TriggerOnlyIfOwningCardIsInPlay = triggerOnlyIfOwningCardIsInPlay;

              var effectValidators = new List<ITargetValidatorFactory>();

              if (effectValidator != null)
                effectValidators.Add(effectValidator);

              self.Targets(effectValidators, Enumerable.Empty<ITargetValidatorFactory>(), selectorAi);
            }
        };
    }

    private static int DefaultManaSourcePriority(ManaAbility ability)
    {
      return ability.OwningCard.Is().Land ? ManaSourcePriorities.Land : ManaSourcePriorities.Creature;
    }
  }
}