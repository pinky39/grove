namespace Grove.Core.Messages
{
  using Details.Cards;
  using Targeting;

  public class PlayerHasActivatedAbility
  {
    public bool HasTarget { get { return Target != null; } }

    public ActivatedAbility Ability { get; set; }
    public ITarget Target { get; set; }
  }
}