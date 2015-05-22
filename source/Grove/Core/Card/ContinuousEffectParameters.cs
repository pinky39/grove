namespace Grove
{
  using System.Collections.Generic;

  public class ContinuousEffectParameters
  {
    public ContinuousEffect.CardSelector Selector = delegate { return true; };
    public bool ApplyOnlyToPermanents = true;    
    public List<CardModifierFactory> Modifiers = new List<CardModifierFactory>();
    public CardModifierFactory Modifier { set { Modifiers.Add(value); } }
  }
}