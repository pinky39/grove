namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using AI.CombatRules;
  using Infrastructure;

  [Copyable]
  public class CardParameters
  {
    public readonly List<ActivatedAbility> ActivatedAbilities = new List<ActivatedAbility>();
    public readonly List<CastRule> CastInstructions = new List<CastRule>();
    public readonly List<CombatRule> CombatRules = new List<CombatRule>();
    public readonly List<CardColor> ProtectionsFromColors = new List<CardColor>();
    public readonly List<string> ProtectionsFromTypes = new List<string>();
    public readonly List<Static> SimpleAbilities = new List<Static>();
    public readonly CardTemplate Template;
    public readonly List<TriggeredAbility> TriggeredAbilities = new List<TriggeredAbility>();

    public List<CardColor> Colors = new List<CardColor>();
    public CardText FlavorText = String.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public List<int> ManaColorsThisCardCanProduce = new List<int>();
    public ManaAmount ManaCost;
    public bool MayChooseToUntap;
    public int? MinBlockerPower = null;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
    public List<StaticAbility> StaticAbilities = new List<StaticAbility>();
    public CardText Text = String.Empty;
    public int? Toughness;
    public int? Loyality;
    public int CombatCost;
    public CardType Type;

    private CardParameters() { }

    public CardParameters(CardTemplate template)
    {
      Template = template;
    }    
    
    public int? Level
    {
      get
      {
        if (IsLeveler)
          return 0;

        return null;
      }
    }

    public string Illustration
    {
      get
      {        
        if (Type.BasicLand)
        {
          return Name + RandomEx.Next(1, Settings.Readonly.BasicLandVersions + 1);
        }

        return Name;
      }
    }
    
    public void Initialize(Card owningCard, Game game)
    {
      foreach (var ability in ActivatedAbilities)
      {
        ability.Initialize(owningCard, game);
      }

      foreach (var ci in CastInstructions)
      {
        ci.Initialize(owningCard, game);
      }

      foreach (var cr in CombatRules)
      {
        cr.Initialize(owningCard, game);
      }

      foreach (var ability in TriggeredAbilities)
      {
        ability.Initialize(owningCard, game);
      }

      foreach (var ability in StaticAbilities)
      {
        ability.Initialize(owningCard, game);
      }
    }
  }
}