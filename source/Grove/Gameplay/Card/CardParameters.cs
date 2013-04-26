namespace Grove.Gameplay.Card
{
  using System;
  using System.Collections.Generic;
  using Abilities;
  using Characteristics;
  using Damage;
  using Mana;

  public class CardParameters
  {
    private static readonly Random Rnd = new Random();
    public readonly ActivatedAbilities ActivatedAbilities = new ActivatedAbilities();
    public readonly CastInstructions CastInstructions = new CastInstructions();
    public readonly ContiniousEffects ContinuousEffects = new ContiniousEffects();
    public readonly Protections Protections = new Protections();
    public readonly StaticAbilities StaticAbilities = new StaticAbilities();
    public readonly TriggeredAbilities TriggeredAbilities = new TriggeredAbilities();
    public List<CardColor> Colors = new List<CardColor>();
    public DamagePreventions DamagePreventions = new DamagePreventions();
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool IsLeveler;
    public IManaAmount ManaCost;
    public string Name;
    public ScoreOverride OverrideScore = new ScoreOverride();
    public int? Power;
    public CardText Text = string.Empty;
    public int? Toughness;
    public CardType Type;
    public bool MayChooseToUntap;

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