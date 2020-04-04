using System.Collections.Generic;

namespace Grove.Costs
{
  public class PayCostParameters
  {
    public bool PayManaCost;
    public Targets Targets;
    public int? X;
    public int Repeat = 1;
    public List<Card> ConvokeTargets;
    public List<Card> DelveTargets;
  }
}