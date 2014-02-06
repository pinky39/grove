namespace Grove.Gameplay.Messages
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Abilities;
  using Targeting;

  public class PlayerHasActivatedAbility : ICardActivationMessage
  {
    private readonly List<ITarget> _targets = new List<ITarget>();

    public PlayerHasActivatedAbility(ActivatedAbility ability, IEnumerable<ITarget> targets)
    {
      Ability = ability;
      _targets.AddRange(targets);
    }

    public bool HasTargets { get { return _targets.Count > 0; } }

    public ActivatedAbility Ability { get; private set; }
    public List<ITarget> Targets { get { return _targets; } }

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
        sb.AppendFormat(" with targets: {0}", string.Join(",", _targets));
      }
      sb.Append(".");
      return sb.ToString();
    }
  }
}