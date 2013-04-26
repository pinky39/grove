namespace Grove.Gameplay.Messages
{
  using System.Collections.Generic;
  using System.Text;
  using Card;
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
    public List<ITarget> Targets { get { return _targets; } }

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