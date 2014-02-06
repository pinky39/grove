namespace Grove.Gameplay.Messages
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Targeting;

  public class PlayerHasCastASpell : ICardActivationMessage
  {
    private readonly List<ITarget> _targets = new List<ITarget>();

    public PlayerHasCastASpell(Card card, IEnumerable<ITarget> targets)
    {
      Card = card;
      _targets.AddRange(targets);
    }

    public bool HasTargets { get { return _targets.Count > 0; } }
    public List<ITarget> Targets { get { return _targets; } }
    public Player Controller { get { return Card.Controller; } }
    
    public string GetTitle()
    {
      return String.Format("{0} casts...", Controller);
    }

    public Card Card { get; private set; }

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