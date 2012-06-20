namespace Grove.Core.Controllers
{
  using Effects;

  public interface IDecisionFactory
  {
    void Initialize(Game game);    
    
    IDecision CreateAssignCombatDamage(Player player, Attacker attacker);
    IDecision CreateDeclareAttackers(Player player);
    IDecision CreateDeclareBlockers(Player player);
    IDecision CreateDiscardCards(Player player, int count);
    IDecision CreatePlaySpellOrAbility(Player player);
    IDecision CreateSacrificeCreatures(Player player, int count);
    IDecision CreateSelectStartingPlayer(Player player);
    IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker);
    IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector);
    IDecision CreateTakeMulligan(Player player);
    IDecision CreateConsiderPayingLifeOrMana(Player player, string question, object ctx, PayLifeOrManaHandler handler, int? life, IManaAmount mana);    
  }
}