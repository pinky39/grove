namespace Grove.Core.Messages
{
  using System.Collections.Generic;
  using System.Text;
  using Targeting;
  
  public class PlayerHasCastASpell
  {
    private readonly List<Target> _targets = new List<Target>();
    
    public PlayerHasCastASpell(Card card, IEnumerable<Target> targets)
    {
      Card = card;
      _targets.AddRange(targets);
    }

    public bool HasTargets { get { return _targets.Count > 0; } }
    public Card Card { get; private set; }
    public IEnumerable<Target> Targets { get { return _targets; } }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("{0} casted {1}", Card.Controller, Card);
      
      if (HasTargets)
      {
        sb.AppendFormat(" with targets: {0}", string.Join(",", _targets));
      }

      sb.Append(".");
      return sb.ToString();
    }
  }
}