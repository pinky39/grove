namespace Grove
{
  using System.Collections.Generic;

  public class ContinuousEffectParameters
  {
    public ShouldApplyToCard CardFilter = delegate { return true; };
    public bool ApplyOnlyToPermaments = true;
    public bool IsCumulative = true;
    public List<CardModifierFactory> Modifiers = new List<CardModifierFactory>();
    public CardModifierFactory Modifier { set { Modifiers.Add(value); } }
  }
}