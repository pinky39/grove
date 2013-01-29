namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Mana;
  using Preventions;

  public class CardParameters
  {
    private static readonly Random Rnd = new Random();
    public readonly ActivatedAbilities ActivatedAbilities = new ActivatedAbilities();
    public readonly List<ContinuousEffect> ContinuousEffects = new List<ContinuousEffect>();
    public readonly List<Static> StaticAbilities = new List<Static>();
    public readonly TriggeredAbilities TriggeredAbilities = new TriggeredAbilities();
    public readonly CastInstructions CastInstructions = new CastInstructions();
    public ManaColors Colors;
    public DamagePrevention[] DamagePrevention = new DamagePrevention[] {};
    public CardText FlavorText = string.Empty;
    public bool Isleveler;
    public IManaAmount ManaCost;
    public bool MayChooseNotToUntap;
    public string Name;
    public int? OverrideScore;
    public int? Power;
    public string[] ProtectionsFromCardTypes;
    public ManaColors ProtectionsFromColors = ManaColors.None;
    public CardText Text = string.Empty;
    public int? Toughness;
    public CardType Type;
    
    public string Illustration
    {
      get
      {
        const int basicLandVersions = 4;

        if (Type.BasicLand)
        {
          return Name + Rnd.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }    
  }
}