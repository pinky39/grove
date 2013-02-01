namespace Grove.Core
{
  using System;
  using Mana;
  using Preventions;

  public class CardParameters
  {
    private static readonly Random Rnd = new Random();
    public readonly ActivatedAbilities ActivatedAbilities = new ActivatedAbilities();    
    public readonly CastInstructions CastInstructions = new CastInstructions();    
    public readonly ContiniousEffects ContinuousEffects = new ContiniousEffects();
    public readonly StaticAbilities StaticAbilities = new StaticAbilities();
    public readonly TriggeredAbilities TriggeredAbilities = new TriggeredAbilities();
    public readonly Protections Protections = new Protections();
    public ManaColors Colors;
    public DamagePreventions DamagePreventions = new DamagePreventions();
    public CardText FlavorText = string.Empty;
    public bool Isleveler;
    public IManaAmount ManaCost;
    public bool MayChooseNotToUntap;
    public string Name;
    public int? OverrideScore;
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
          return Name + Rnd.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }
  }
}