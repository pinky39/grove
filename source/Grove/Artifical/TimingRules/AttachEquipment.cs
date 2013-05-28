namespace Grove.Artifical.TimingRules
{
  using System;
  using Gameplay.States;

  [Serializable]
  public class AttachEquipment : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.IsAttached)
      {
        // reattach to blocker
        return Turn.Step == Step.SecondMain;
      }

      // attach to attacker
      return Turn.Step == Step.FirstMain;
    }
  }
}