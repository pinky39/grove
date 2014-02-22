namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using AI.CombatRules;
  using Infrastructure;

  public class CardParameters
  {
    public readonly List<ActivatedAbility> ActivatedAbilities = new List<ActivatedAbility>();
    public readonly List<CastInstruction> CastInstructions = new List<CastInstruction>();
    public readonly List<CombatRule> CombatRules = new List<CombatRule>();
    public readonly List<ContinuousEffect> ContinuousEffects = new List<ContinuousEffect>();
    public readonly List<string> ProtectionsFromTypes = new List<string>();
    public readonly List<CardColor> ProtectionsFromColors = new List<CardColor>();
    public readonly List<Static> SimpleAbilities = new List<Static>();
    public readonly List<TriggeredAbility> TriggeredAbilities = new List<TriggeredAbility>();
    public List<CardColor> Colors = new List<CardColor>();
    public List<int> ManaColorsThisCardCanProduce = new List<int>();
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public IManaAmount ManaCost;
    public bool MayChooseToUntap;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
    public List<StaticAbility> StaticAbilities = new List<StaticAbility>();
    public CardText Text = string.Empty;
    public int? Toughness;
    public CardType Type;
    public int MinimalBlockerCount = 1;

    public string Illustration
    {
      get
      {
        const int basicLandVersions = 4;

        if (Type.BasicLand)
        {
          return Name + RandomEx.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }
  }
}