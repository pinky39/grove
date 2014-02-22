namespace Grove.Gameplay.AI.RepetitionRules
{
  using System;

  public class RepeatForOptimalMassDamage : RepetitionRule
  {
    public override int GetRepetitionCount(RepetitionRuleParameters p)
    {
      var opponent = p.Card.Controller.Opponent;
      var controller = p.Card.Controller;

      var maxToTry = Math.Min(controller.Life, p.MaxRepetitions);
      var result = MassRemovalParameterOptimizer.CalculateOptimalDamage(controller, opponent, maxToTry);
      return result;
    }
  }
}