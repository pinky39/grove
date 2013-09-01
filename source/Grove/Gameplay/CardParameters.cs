namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Abilities;
  using Artifical.CombatRules;
  using Characteristics;
  using Infrastructure;
  using ManaHandling;

  public class CardParameters
  {
    public readonly ActivatedAbilities ActivatedAbilities = new ActivatedAbilities();
    public readonly CastInstructions CastInstructions = new CastInstructions();
    public readonly CombatRules CombatRules = new CombatRules();
    public readonly ContiniousEffects ContinuousEffects = new ContiniousEffects();
    public readonly Protections Protections = new Protections();
    public readonly SimpleAbilities SimpleAbilities = new SimpleAbilities();
    public readonly TriggeredAbilities TriggeredAbilities = new TriggeredAbilities();
    public List<CardColor> Colors = new List<CardColor>();
    public List<CardColor> ManaColorsThisCardCanProduce = new List<CardColor>();
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public IManaAmount ManaCost;
    public bool MayChooseToUntap;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
    public StaticAbilities StaticAbilities = new StaticAbilities();
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