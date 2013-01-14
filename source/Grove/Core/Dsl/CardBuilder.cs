namespace Grove.Core.Dsl
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Casting;
  using Costs;
  using Counters;
  using Effects;
  using Mana;
  using Modifiers;
  using Preventions;
  using Redirections;
  using Targeting;
  using Triggers;
  using Zones;

  public class CardBuilder
  {
    public CardFactory Card { get { return new CardFactory(); } }

    public ICastingRuleFactory Rule<T>(Initializer<T> init = null) where T : CastingRule, new()
    {
      init = init ?? delegate { };
      return new CastingRule.Factory<T>()
        {
          Init = init
        };
    }

    public IActivatedAbilityFactory ActivatedAbility(
      string text,
      ICostFactory cost,
      Effects.IEffectFactory effect,
      ITargetValidatorFactory effectTarget = null,
      ITargetValidatorFactory costTarget = null,
      TargetingAiDelegate targetingAi = null,
      bool activateAsSorcery = false,
      EffectCategories category = EffectCategories.Generic,
      TimingDelegate timing = null,
      Zone activationZone = Zone.Battlefield)
    {
      var effectSelectors = effectTarget == null
        ? new ITargetValidatorFactory[] {}
        : new[] {effectTarget};

      return ActivatedAbility(text, cost, effect, effectSelectors, costTarget,
        targetingAi, activateAsSorcery, category, timing, activationZone);
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
      Effects.IEffectFactory effect,
      ITargetValidatorFactory[] effectTargets,
      ITargetValidatorFactory costTarget = null,
      TargetingAiDelegate targetingAi = null,
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

              if (costTarget != null)
                costValidators.Add(costTarget);

              self.Targets(effectTargets, costValidators, targetingAi);

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

    public ICostFactory Cost<T1, T2>(Initializer<T1> init1 = null, Initializer<T2> init2 = null) where T1 : Cost, new()
      where T2 : Cost, new()
    {
      init1 = init1 ?? delegate { };
      init2 = init2 ?? delegate { };
      var cost1 = new Cost.Factory<T1>
        {
          Init = init1
        };

      var cost2 = new Cost.Factory<T2>()
        {
          Init = init2
        };

      return new Cost.Factory<AggregateCost>()
        {
          Init = cost => cost.CostsFactories = new List<ICostFactory> {cost1, cost2}
        };
    }

    public ICostFactory Cost<T1, T2, T3>(Initializer<T1> init1 = null, Initializer<T2> init2 = null,
      Initializer<T3> init3 = null)
      where T1 : Cost, new()
      where T2 : Cost, new()
      where T3 : Cost, new()
    {
      init1 = init1 ?? delegate { };
      init2 = init2 ?? delegate { };
      init3 = init3 ?? delegate { };

      var cost1 = new Cost.Factory<T1>
        {
          Init = init1
        };

      var cost2 = new Cost.Factory<T2>()
        {
          Init = init2
        };

      var cost3 = new Cost.Factory<T3>()
        {
          Init = init3
        };

      return new Cost.Factory<AggregateCost>()
        {
          Init = cost => cost.CostsFactories = new List<ICostFactory> {cost1, cost2, cost3}
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

    public Effects.IEffectFactory Effect<T>(Action<T> init = null) where T : Effects.Effect, new()
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
      return ManaAbility(delegate { return manaAmount; }, text, cost, priority);
    }

    public IActivatedAbilityFactory ManaAbility(Func<ManaAbility, Game, IManaAmount> manaAmount, string text,
      ICostFactory cost = null,
      int? priority = null)
    {
      cost = cost ?? new Cost.Factory<Tap>();

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

    public ITargetValidatorFactory Target(TargetValidatorDelegate target, ZoneValidatorDelegate zone,
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
      TargetingAiDelegate selectorAi = null,
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