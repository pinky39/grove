namespace Grove
{
  using Modifiers;

  public class SkipStep : Modifier, IPlayerModifier
  {
    private readonly Step _step;
    private SkipSteps _skipSteps;

    private SkipStep() {}

    public SkipStep(Step step)
    {
      _step = step;
    }

    public override void Apply(SkipSteps skipSteps)
    {
      _skipSteps = skipSteps;
      skipSteps.Add(_step);
    }

    protected override void Unapply()
    {
      _skipSteps.Remove(_step);
    }
  }
}