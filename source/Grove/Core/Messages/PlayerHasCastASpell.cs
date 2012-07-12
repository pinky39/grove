namespace Grove.Core.Messages
{
  using System.Collections.Generic;
  using System.Text;
  using Targeting;
  
  public class PlayerHasCastASpell
  {
    private readonly List<ITarget> _targets = new List<ITarget>();
    
    public PlayerHasCastASpell(Card card, IEnumerable<ITarget> targets)
    {
      Card = card;
      _targets.AddRange(targets);
    }

    public bool HasTargets { get { return _targets.Count > 0; } }
    public Card Card { get; private set; }
    public IEnumerable<ITarget> Targets { get { return _targets; } }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Player: '{0}' has cast {1}", Card.Controller, Card);
      
      if (HasTargets)
      {
        sb.AppendFormat(" with targets: {0}", string.Join(",", _targets));
      }

      sb.Append(".");
      return sb.ToString();
    }
  }
}