namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Costs;
  using Effects;
  using Events;
  using Infrastructure;

  public class ActivatedAbility : Ability, IHashable
  {
    private readonly Trackable<int> _lastActivation = new Trackable<int>();
    private readonly ActivatedAbilityParameters _p;

    protected ActivatedAbility() {}

    public ActivatedAbility(ActivatedAbilityParameters p)
      : base(p)
    {
      _p = p;
    }

    public virtual void OnEnable() {}

    public virtual void OnDisable() {}

    public void Activate(ActivationParameters p)
    {
      _lastActivation.Value = Turn.TurnCount;

      var effects = new List<Effect>();

      for (var i = 0; i < p.Repeat; i++)
      {
        // create effect first, since some cost e.g Sacrifice can
        // put owning card to graveyard which will alter some card
        // properties e.g counters, power, toughness ...             
        var effect = CreateEffect();

        var effectParameters = new EffectParameters
          {
            Source = this,
            Targets = p.Targets,
            X = p.X
          };

        effect.Initialize(effectParameters, Game);
        effects.Add(effect);
      }

      Pay(p);

      foreach (var effect in effects)
      {
        if (p.SkipStack)
        {
          Resolve(effect, true);
          continue;
        }

        Publish(new AbilityActivatedEvent(this, p.Targets));

        Resolve(effect, false);
        Publish(new ActivatedAbilityPutOnStackEvent(this, p.Targets));
      }
    }    

    public ManaAmount GetManaCost()
    {
      return _p.Cost.GetManaCost();
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        _p.ActivateAsSorcery.GetHashCode(),
        calc.Calculate(_p.Cost));
    }

    public ActivationPrerequisites GetPrerequisites()
    {
      var prerequisites = new ActivationPrerequisites
      {
        CanBePlayedAtThisTime = CanBeActivatedAtThisTime(),                  
      };

      // mana check is rather expensive, if 
      // card cant be cast we can skip it
      if (!prerequisites.CanBePlayed)
        return prerequisites;

      var canPay = CanPay();

      prerequisites.CanBePayed = canPay.CanPay;
      prerequisites.Description = _p.Text;
      prerequisites.Selector = _p.TargetSelector;
      prerequisites.MaxX = canPay.MaxX;
      prerequisites.DistributeAmount = _p.DistributeAmount;
      prerequisites.Card = OwningCard;
      prerequisites.Rules = _p.Rules;
      prerequisites.MaxRepetitions = canPay.MaxRepetitions;

      return prerequisites;
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);
      _p.Cost.Initialize(CostType.Ability, owningCard, game, _p.TargetSelector.Cost.FirstOrDefault());      
      _lastActivation.Initialize(ChangeTracker);
    }

    protected virtual Effect CreateEffect()
    {
      return _p.Effect();
    }

    protected void Pay(ActivationParameters p = null)
    {
      p = p ?? new ActivationParameters();
      
      _p.Cost.Pay(new PayCostParameters
        {
          Targets =  p.Targets, 
          X = p.X, 
          Repeat = p.Repeat, 
          PayManaCost = p.PayManaCost
        });      
    }

    protected CanPayResult CanPay()
    {
      return _p.Cost.CanPay(payManaCost: true);
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != _p.ActivationZone)
        return false;

      if (_p.ActivateOnlyOnceEachTurn && _lastActivation.Value == Turn.TurnCount)
      {
        return false;
      }

      if (!_p.Condition(OwningCard, Game))
      {
        return false;
      }

      if (OwningCard.Is().Planeswalker && Turn.Events.HasAnyLoyalityAbilityBeenActivated(OwningCard))
      {
        return false;
      }

      if (_p.ActivateAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
          Stack.IsEmpty;
      }

      return true;
    }    
  }
}