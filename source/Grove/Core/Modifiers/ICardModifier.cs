namespace Grove.Modifiers
{
  public interface ICardModifier : IModifier
  {
    void Apply(CardController controller);
    void Apply(TriggeredAbilities abilities);
    void Apply(SimpleAbilities abilities);
    void Apply(ActivatedAbilities abilities);
    void Apply(CardColors colors);    
    void Apply(Protections protections);
    void Apply(CardTypeCharacteristic cardType);
    void Apply(Counters counters);
    void Apply(Level level);
    void Apply(ContiniousEffects continiousEffects);
    void Apply(Strenght strenght);
  }
}