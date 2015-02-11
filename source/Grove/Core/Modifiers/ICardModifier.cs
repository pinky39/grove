namespace Grove.Modifiers
{
  public interface ICardModifier : IModifier
  {
    void Apply(CardController controller);
    void Apply(TriggeredAbilities abilities);
    void Apply(SimpleAbilities abilities);
    void Apply(StaticAbilities abilities);
    void Apply(ActivatedAbilities abilities);
    void Apply(CardColors colors);    
    void Apply(Protections protections);
    void Apply(CardTypeCharacteristic cardType);
    void Apply(Counters counters);
    void Apply(Level level);    
    void Apply(Strenght strenght);
    void Apply(MinBlockerCount count);
    void Apply(CombatCost combatCost);
    void Apply(CardBase cardBase);
  }
}