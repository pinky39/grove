namespace Grove.Core.Details.Cards
{
  using Infrastructure;
  using Targeting;

  [Copyable]
  public class ActivationParameters
  {
    private readonly Targets _targets;    

    public ActivationParameters(Targets targets = null, bool payKicker = false, int? x = null)
    {
      _targets = targets ?? new Targets();
      PayKicker = payKicker;
      X = x;      
    }

    private ActivationParameters() {}

    public Targets Targets { get { return _targets; } }
    public static ActivationParameters Default { get { return new ActivationParameters(); } }

    public bool PayKicker { get; private set; }
    public int? X { get; private set; }    
  }
}