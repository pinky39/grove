namespace Grove.Gameplay.Triggers
{
  using System.Linq;
  using Infrastructure;
  using Messages;

  public class OnBeingTargetedBySpellOrAbility : Trigger, IReceive<EffectPushedOnStack>
  {
    public void Receive(EffectPushedOnStack message)
    {
      if (message.Effect.Targets.Any(target => target == Ability.OwningCard))
      {
        Set(message);
      }
    }
  }
}