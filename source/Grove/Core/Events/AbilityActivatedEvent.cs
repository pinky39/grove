namespace Grove.Events
{
  using System;
  using System.Text;

  public class AbilityActivatedEvent : ICardActivationEvent
  {
    public readonly ActivatedAbility Ability;
    public readonly Targets Targets;

    public AbilityActivatedEvent(ActivatedAbility ability, Targets targets)
    {
      Ability = ability;
      Targets = targets;
    }

    public bool HasTargets { get { return Targets.Count > 0; } }
    public Player Controller { get { return Ability.SourceCard.Controller; } }

    public string GetTitle()
    {
      return String.Format("{0} activates...", Controller);
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("{0} activated {1}", Ability.OwningCard.Controller, Ability);
      if (HasTargets)
      {
        sb.AppendFormat(" with targets: {0}", string.Join(",", Targets));
      }
      sb.Append(".");
      return sb.ToString();
    }
  }
}