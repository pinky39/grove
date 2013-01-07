namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Cards;
  using Cards.Effects;
  using Infrastructure;
  using Targeting;

  [Copyable]
  public class CastInstructions
  {
    private readonly List<CastInstruction> _castInstructions = new List<CastInstruction>();

    public CastInstructions(IEnumerable<CastInstruction> castInstructions)
    {
      _castInstructions.AddRange(castInstructions);
    }

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
        var activation = new ActivationParameters(new Targets().AddEffect(target));
        var effect = castInstruction.CreateEffect(activation) as T;

        if (effect != null)
          return effect;
      }

      return null;
    }
  }
}