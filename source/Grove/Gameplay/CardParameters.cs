namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Abilities;
  using Characteristics;
  using Damage;
  using Infrastructure;
  using ManaHandling;
  using Modifiers;

  public class CardParameters
  {
    public readonly ActivatedAbilities ActivatedAbilities = new ActivatedAbilities();
    public readonly CastInstructions CastInstructions = new CastInstructions();
    public readonly ContiniousEffects ContinuousEffects = new ContiniousEffects();
    public readonly List<IModifier> Modifiers = new List<IModifier>();
    public readonly Protections Protections = new Protections();
    public readonly StaticAbilities StaticAbilities = new StaticAbilities();
    public readonly TriggeredAbilities TriggeredAbilities = new TriggeredAbilities();
    public List<CardColor> Colors = new List<CardColor>();
    public DamagePreventions DamagePreventions = new DamagePreventions();
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public IManaAmount ManaCost;
    public bool MayChooseToUntap;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
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
          return Name + RandomEx.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }
  }
}