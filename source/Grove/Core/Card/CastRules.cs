namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  [Copyable]
  public class CastRules
  {
    private readonly List<CastRule> _castRules = new List<CastRule>();

    private CastRules() { }

    public CastRules(IEnumerable<CastRule> castRules) { _castRules.AddRange(castRules); }

    public bool HasXInCost { get { return _castRules.Any(x => x.HasXInCost); } }
    public int Count { get { return _castRules.Count; } }

    public List<ActivationPrerequisites> CanCast()
    {
      var allPrerequisites = new List<ActivationPrerequisites>();

      for (var index = 0; index < _castRules.Count; index++)
      {
        var instruction = _castRules[index];
        ActivationPrerequisites prerequisites;

        if (instruction.CanCast(out prerequisites))
        {
          prerequisites.Index = index;
          allPrerequisites.Add(prerequisites);
        }
      }

      return allPrerequisites;
    }

    public void Initialize(Card card, Game game)
    {
      foreach (var castInstruction in _castRules)
      {
        castInstruction.Initialize(card, game);
      }
    }

    public void Cast(int index, ActivationParameters activationParameters) { _castRules[index].Cast(activationParameters); }
    public bool CanTarget(ITarget target) { return _castRules.Any(x => x.CanTarget(target)); }
    public bool IsGoodTarget(ITarget target, Player controller) { return _castRules.Any(x => x.IsGoodTarget(target, controller)); }
    public IManaAmount GetManaCost(int index) { return _castRules[index].GetManaCost(); }
  }
}