namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Cards;
  using Cards.Effects;
  using Infrastructure;
  using Mana;
  using Targeting;

  [Copyable]
  public class CastInstructions
  {
    private readonly List<CastInstruction> _castInstructions = new List<CastInstruction>();

    public CastInstructions(IEnumerable<CastInstruction> castInstructions)
    {
      _castInstructions.AddRange(castInstructions);
    }

    private CastInstructions() {}

    public List<SpellPrerequisites> CanCast()
    {
      return _castInstructions.Select(x => x.CanCast()).ToList();
    }

    public void Cast(int index, ActivationParameters activationParameters)
    {
      _castInstructions[index].Cast(activationParameters);
    }

    public bool CanTarget(ITarget target)
    {
      return _castInstructions.Any(x => x.CanTarget(target));
    }

    public void EnchantTarget(Card target)
    {
      var effect = CreateEffect<Attach>(target);      
      effect.Resolve();
      effect.FinishResolve();
    }

    private T CreateEffect<T>(ITarget target) where T : Effect
    {
      foreach (var castInstruction in _castInstructions)
      {
        var activation = new ActivationParameters();
        activation.Targets.AddEffect(target);
        var effect = castInstruction.CreateEffect(activation) as T;

        if (effect != null)
          return effect;
      }

      return null;
    }

    public bool IsGoodTarget(ITarget target)
    {
      return _castInstructions.Any(x => x.IsGoodTarget(target));      
    }

    public IManaAmount GetManaCost(int index)
    {
      return _castInstructions[index].GetManaCost();
    }
  }
}