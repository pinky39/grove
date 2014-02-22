namespace Grove.Gameplay.Decisions
{
  using System;

  public class Verify
  {
    public readonly Action Assertion;

    public Verify(Action assertion)
    {
      Assertion = assertion;
    }
  }
}