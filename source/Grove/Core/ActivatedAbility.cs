namespace Grove.Core
{
  using System;
  using Ai;
  using Costs;
  using Effects;
  using Infrastructure;
  using Messages;

  public class ActivatedAbility : Ability
  {
    private TimingDelegate _timming = Timings.MainPhases();

    public ActivatedAbility()
    {
      UsesStack = true;
    }

    public bool ActivateOnlyAsSorcery { get; set; }    
    public bool TargetsSelf { get; set; }
    protected Cost Cost { get; private set; }

    public IManaAmount ManaCost
    {
      get
      {
        var manaCost = Cost as TapOwnerPayMana;

        return manaCost == null
          ? ManaAmount.Zero
          : manaCost.Amount ?? ManaAmount.Zero;
      }
    }

    protected TurnInfo Turn { get { return Game.Turn; } }

    public void Activate(ActivationParameters activation)
    {
      Cost.Pay(activation.Targets.Cost(0), activation.X);
      var effect = EffectFactory.CreateEffect(this, activation.X);

      effect.AddTargets(activation.Targets.Effect());      
            
      Publisher.Publish(new PlayerHasActivatedAbility{
        Ability = this,
        Target = effect.Target
      });

      if (UsesStack)
      {
        Stack.Push(effect);
        return;
      }

      effect.Resolve();
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(Cost),
        calc.Calculate(EffectFactory),
        ActivateOnlyAsSorcery.GetHashCode());
    }

    public virtual SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      var canActivate = CanBeActivated(ref maxX);

      return canActivate
        ? new SpellPrerequisites{          
          CanBeSatisfied = true,
          Description = Text,    
          TargetSelectors = TargetSelectors,                
          XCalculator = Cost.XCalculator,
          MaxX = maxX,
          Timming = _timming,
          TargetsSelf =  TargetsSelf,
        }
        : new SpellPrerequisites{
          CanBeSatisfied = false
        };
    }

    public T GetEffect<T>() where T : Effect
    {
      return EffectFactory.CreateEffect(this) as T;
    }

    public void SetCost(ICostFactory costFactory)
    {
      Cost = costFactory.CreateCost(this);
    }

    public void Timing(TimingDelegate timing)
    {
      if (timing != null)
        _timming = timing;
    }

    private bool CanBeActivated(ref int? maxX)
    {
      return
        CanBeActivatedAtThisTime() &&
          Cost.CanPay(ref maxX);
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (ActivateOnlyAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
            Stack.IsEmpty;
      }

      return true;
    }

    public class Factory<T> : IActivatedAbilityFactory where T : ActivatedAbility, new()
    {
      public Action<T> Init = delegate { };
      public Game Game { get; set; }

      public ActivatedAbility Create(Card card)
      {
        var ability = new T();
        ability.OwningCard = card;
        ability.SourceCard = card;
        ability.Game = Game;

        Init(ability);

        return ability;
      }
    }
  }
}