namespace Grove.Core
{
  using System;
  using System.Linq;
  using Ai;
  using Cards;
  using Cards.Casting;
  using Cards.Costs;
  using Cards.Effects;
  using Infrastructure;
  using Messages;
  using Targeting;
  using Zones;

  [Copyable]
  public class CastInstruction : IEffectSource
  {
    private readonly Card _card;
    private readonly CastingRule _castingRule;
    private readonly Cost _cost;
    private readonly string _description;
    private readonly bool _distributeDamage;
    private readonly EffectCategories _effectCategories;
    private readonly IEffectFactory _effectFactory;
    private readonly Game _game;
    private readonly TargetSelector _targetSelector;
    private readonly TimingDelegate _timing;
    private readonly CalculateX _xCalculator;

    public CastInstruction(Card card, Game game, CastInstructionParameters p)
    {
      _card = card;
      _game = game;

      _targetSelector = new TargetSelector(
        p.EffectTargets.Select(x => x.Create(card, game)),
        p.CostTargets.Select(x => x.Create(card, game)),
        p.TargetSelectorAi);

      _cost = p.Cost.CreateCost(
        card, _targetSelector.Cost.FirstOrDefault(), _game);

      _castingRule = p.Rule.CreateCastingRule(card, _game);

      _effectFactory = p.Effect;
      _description = p.Description;

      _xCalculator = p.XCalculator;
      _distributeDamage = p.DistributeDamage;
      _timing = p.Timing;
      _effectCategories = p.Category;
    }


    public Card OwningCard { get { return _card; } }
    public Card SourceCard { get { return _card; } }

    public void EffectCountered(SpellCounterReason reason)
    {
      _card.PutToGraveyard();
    }

    public void EffectPushedOnStack()
    {
      _card.ChangeZone(_game.Stack);

      _game.Publish(new CardChangedZone
        {
          Card = _card,
          From = Zone.Hand,
          To = Zone.Stack
        });
    }

    public void EffectResolved()
    {
      _castingRule.AfterResolve();     
    }

    public bool IsTargetStillValid(ITarget target, bool wasKickerPaid)
    {
      return _targetSelector.IsValidEffectTarget(target);
    }

    public SpellPrerequisites CanCast()
    {
      int? maxX = null;

      if (_castingRule.CanCast() == false || _cost.CanPay(ref maxX) == false)
      {
        return SpellPrerequisites.CannotBeSatisfied;
      }

      return new SpellPrerequisites
        {
          CanBeSatisfied = true,
          Description = _description,
          TargetSelector = _targetSelector,
          MaxX = maxX,
          DistributeDamage = _distributeDamage,
          XCalculator = _xCalculator,
          Timing = _timing,
        };
    }

    public void Cast(ActivationParameters activationParameters)
    {      
      var effect = CreateEffect(activationParameters);
      _castingRule.Cast(effect);
      _game.Publish(new PlayerHasCastASpell(_card, activationParameters.Targets));
    }

    public Effect CreateEffect(ActivationParameters activationParameters)
    {
      var parameters = new EffectParameters(this, _effectCategories, activationParameters);
      return _effectFactory.CreateEffect(parameters, _game);
    }        

    public bool CanTarget(ITarget target)
    {
      return _targetSelector.Effect[0].IsValid(target);
    }
  }
}