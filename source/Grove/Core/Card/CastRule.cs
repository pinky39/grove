namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using Costs;
  using Effects;
  using Events;

  public class CastRule : GameObject, IEffectSource
  {
    private readonly Parameters _p;
    private Card _card;

    private CastRule() {}

    public CastRule(Parameters p)
    {
      _p = p;
    }

    public bool HasXInCost
    {
      get { return _p.Cost.HasX; }
    }

    public Card OwningCard
    {
      get { return _card; }
    }

    public Card SourceCard
    {
      get { return _card; }
    }

    public void EffectCountered(SpellCounterReason reason)
    {
      _card.PutToGraveyard();

      Publish(new SpellCounteredEvent(_card, reason));
    }

    public void EffectPushedOnStack()
    {
      _card.ChangeZone(
        destination: Stack,
        add: delegate { });
    }

    public void EffectResolved()
    {
      var putToZone = _p.AfterResolve ?? PutToZoneAfterResolve;
      putToZone(_card);
    }

    public bool IsTargetStillValid(ITarget target, object triggerMessage)
    {
      return _p.TargetSelector.IsValidEffectTarget(target, triggerMessage);
    }

    public bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets)
    {
      return _p.TargetSelector.ValidateTargetDependencies(new ValidateTargetDependenciesParam
        {
          Cost = costTargets,
          Effect = effectTargets
        });
    }

    private void PutToZoneAfterResolve(Card card)
    {
      if (card.Is().Sorcery || card.Is().Instant)
      {
        card.PutToGraveyard();
        return;
      }

      if (card.Is().Aura)
      {
        var attachedCardController = card.GetControllerOfACardThisIsAttachedTo();
        attachedCardController.PutCardToBattlefield(card);
        return;
      }

      card.PutToBattlefield();
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;
      _card = card;

      _p.TargetSelector.Initialize(card, game);

      foreach (var aiInstruction in _p.Rules)
      {
        aiInstruction.Initialize(game);
      }

      _p.Cost.Initialize(CostType.Spell, card, game, _p.TargetSelector.Cost.FirstOrDefault());
    }

    private bool CanCastCardType()
    {
      if (_card.Is().Instant || _card.Has().Flash)
        return true;

      if (!_card.Controller.IsActive || !Turn.Step.IsMain() || !Stack.IsEmpty)
        return false;

      return !_card.Is().Land || _card.Controller.CanPlayLands;
    }

    public bool CanCast(out ActivationPrerequisites prerequisites)
    {
      prerequisites = null;

      if (CanCastCardType() == false)
      {
        return false;
      }

      if (_p.Condition(_card, Game) == false)
      {
        return false;
      }

      var result = _p.Cost.CanPay();

      prerequisites = new ActivationPrerequisites
        {
          CanPay = result.CanPay,
          Description = _p.Text,
          Selector = _p.TargetSelector,
          MaxX = result.MaxX,
          DistributeAmount = _p.DistributeAmount,
          Card = _card,
          Rules = _p.Rules
        };

      return true;
    }

    public void Cast(ActivationParameters p)
    {
      var parameters = new EffectParameters
        {
          Source = this,
          Targets = p.Targets,
          X = p.X
        };

      var effect = _p.Effect().Initialize(parameters, Game);

      if (p.PayCost)
      {
        _p.Cost.Pay(new PayCostParameters {Targets = p.Targets, X = p.X});
      }

      if (p.SkipStack)
      {
        effect.QuickResolve();
        return;
      }

      if (_card.Is().Land)
      {
        _card.Controller.LandsPlayedCount++;

        effect.QuickResolve();
        Publish(new LandPlayedEvent(effect.Source.OwningCard));

        return;
      }

      Publish(new SpellCastEvent(_card, p.Targets));
      Stack.Push(effect);
      Publish(new SpellPutOnStackEvent(effect));
    }

    public bool CanTarget(ITarget target)
    {
      return _p.TargetSelector.Effect[0].IsTargetValid(target, _card);
    }

    public bool IsGoodTarget(ITarget target, Player controller)
    {
      return TargetingHelper.IsGoodTargetForSpell(
        target, OwningCard, controller, _p.TargetSelector,
        _p.Rules.Where(r => r is TargetingRule).Cast<TargetingRule>());
    }

    public ManaAmount GetManaCost()
    {
      return _p.Cost.GetManaCost();
    }

    public override string ToString()
    {
      return _card.ToString();
    }

    public class Parameters : AbilityParameters
    {
      public Cost Cost;
      public string KickerDescription = "Cast {0} with kicker.";
      public Action<Card> AfterResolve;
      public Func<Card, Game, bool> Condition = delegate { return true; };
    }
  }
}