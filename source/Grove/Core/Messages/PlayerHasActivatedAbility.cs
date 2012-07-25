namespace Grove.Core.Messages
{
  using System.Collections.Generic;
  using System.Text;
  using Details.Cards;
  using Targeting;

  public class PlayerHasActivatedAbility
  {
    private readonly List<ITarget> _targets = new List<ITarget>();

    public PlayerHasActivatedAbility(ActivatedAbility ability, IEnumerable<ITarget> targets)
    {
      Ability = ability;      
      _targets.AddRange(targets);
    }

    public bool HasTargets { get { return _targets.Count > 0; } }

    public ActivatedAbility Ability { get; private set; }
    public IEnumerable<ITarget> Targets { get { return _targets; } }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("{0} activated {1}", Ability.Controller, Ability);
      if (HasTargets)
      {
        sb.AppendFormat(" with targets: {0}", string.Join(",", _targets));
      }
      sb.Append(".");
      return sb.ToString();
    }
  }
}