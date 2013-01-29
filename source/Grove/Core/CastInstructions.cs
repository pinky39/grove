namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Targeting;

  [Copyable]
  public class CastInstructions
  {
    private readonly List<CastInstruction> _castInstructions = new List<CastInstruction>();

    public List<ActivationPrerequisites> CanCast()
    {
      var allPrerequisites = new List<ActivationPrerequisites>();

      for (var index = 0; index < _castInstructions.Count; index++)
      {
        var instruction = _castInstructions[index];
        ActivationPrerequisites prerequisites;

        if (instruction.CanCast(out prerequisites))
        {
          prerequisites.Index = index;
          allPrerequisites.Add(prerequisites);
        }
      }

      return allPrerequisites;
    }

    public void Cast(int index, ActivationParameters activationParameters)
    {
      _castInstructions[index].Cast(activationParameters);
    }

    public bool CanTarget(ITarget target)
    {
      return _castInstructions.Any(x => x.CanTarget(target));
    }

    public bool IsGoodTarget(ITarget target)
    {
      return _castInstructions.Any(x => x.IsGoodTarget(target));
    }

    public IManaAmount GetManaCost(int index)
    {
      return _castInstructions[index].GetManaCost();
    }

    public void Add(CastInstruction castInstruction)
    {
      _castInstructions.Add(castInstruction);
    }
  }
}