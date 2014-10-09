namespace Grove.Effects
{
  using System.Linq;

  public class BooleanEffectChoice : IEffectChoice
  {
    public BooleanEffectChoice()
    {
      Options = new object[] {"True", "False"};
    }

    public object[] Options { get; private set; }
  }
}
