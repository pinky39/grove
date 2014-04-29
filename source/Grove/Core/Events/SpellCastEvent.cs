namespace Grove.Events
{
  using System;
  using System.Text;

  public class SpellCastEvent : ICardActivationEvent
  {
    public readonly Card Card;
    public readonly Targets Targets;

    public SpellCastEvent(Card card, Targets targets)
    {
      Card = card;
      Targets = targets;
    }

    public bool HasTargets { get { return Targets.Count > 0; } }
    public Player Controller { get { return Card.Controller; } }

    public string GetTitle()
    {
      return String.Format("{0} casts...", Controller);
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("{0} casted {1}", Card.Controller, Card);

      if (HasTargets)
      {
        sb.AppendFormat(" with targets: {0}", string.Join(",", Targets));
      }

      sb.Append(".");
      return sb.ToString();
    }
  }
}