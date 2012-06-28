namespace Grove.Core.Messages
{
  public class PlayerHasActivatedAbility
  {
    public bool HasTarget
    {
      get { return Target != null; }
    }

    public ActivatedAbility Ability { get; set; }
    public ITarget Target { get; set; }
  }
}