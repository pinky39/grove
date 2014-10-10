namespace Grove
{
  using System;
  using System.Collections.Generic;
  using AI.CombatRules;
  using Infrastructure;

  public class CardParameters
  {
    public readonly List<ActivatedAbility> ActivatedAbilities = new List<ActivatedAbility>();
    public readonly List<CastRule> CastInstructions = new List<CastRule>();
    public readonly List<CombatRule> CombatRules = new List<CombatRule>();
    public readonly List<ContinuousEffect> ContinuousEffects = new List<ContinuousEffect>();
    public readonly List<CardColor> ProtectionsFromColors = new List<CardColor>();
    public readonly List<string> ProtectionsFromTypes = new List<string>();
    public readonly List<Static> SimpleAbilities = new List<Static>();
    public readonly List<TriggeredAbility> TriggeredAbilities = new List<TriggeredAbility>();
    public List<CardColor> Colors = new List<CardColor>();
    public CardText FlavorText = String.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public List<int> ManaColorsThisCardCanProduce = new List<int>();
    public IManaAmount ManaCost;
    public bool MayChooseToUntap;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
    public List<StaticAbility> StaticAbilities = new List<StaticAbility>();
    public CardText Text = String.Empty;
    public int? Toughness;
    public CardType Type;

    public string Illustration
    {
      get
      {
        const int basicLandVersions = 15;

        if (Type.BasicLand)
        {
          return Name + RandomEx.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }
  }
}