namespace Grove.Gameplay.Effects
{
  using System.Linq;

  public class DiscreteEffectChoice : IEffectChoice
  {
    public DiscreteEffectChoice(params EffectOption[] options)
    {
      Options = options.Select(x => (object) x).ToArray();
    }

    public DiscreteEffectChoice(params string[] options)
    {
      Options = options.Select(x => (object) x).ToArray();
    }

    public object[] Options { get; private set; }
  }
}