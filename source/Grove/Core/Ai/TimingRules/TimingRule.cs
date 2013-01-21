namespace Grove.Core.Ai.TimingRules
{
  public abstract class TimingRule : MachinePlayRule
  {
    public override void Process(ActivationContext context)
    {
       if (ShouldPlay(context) == false)
       {
         context.CancelActivation = true;
       }
    }

    public abstract bool ShouldPlay(ActivationContext context);
  }
}