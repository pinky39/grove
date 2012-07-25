namespace Grove.Core
{
  using System.Collections.Generic;
  using Details.Cards.Modifiers;
  using Details.Mana;
  using Infrastructure;
  using Targeting;
  using Zones;

  public interface IPlayer : IHashable, IDamageable
  {
    string Avatar { get; }
    IBattlefieldQuery Battlefield { get; }
    int ConvertedMana { get; }
    bool CanPlayLands { get; set; }
    IHandQuery Hand { get; }
    bool HasPriority { get; set; }
    bool IsActive { get; set; }
    bool IsHuman { get; }
    bool IsComputer { get; }
    bool IsScenario { get; }
    bool IsMax { get; set; }
    IEnumerable<Card> Library { get; }
    int Life { get; set; }
    int Score { get; }
    bool HasLost { get; set; }
    bool HasMulligan { get; set; }
    int NumberOfCardsAboveMaximumHandSize { get; }
    IEnumerable<Card> Graveyard { get; }
    bool CanMulligan { get; }    
    bool HasMana(int amount);
    bool HasMana(IManaAmount amount);
    void Consume(IManaAmount amount, IManaSource tryNotToConsumeThisSource = null);
    void DiscardHand();
    void DiscardRandomCard();
    void DrawCard();
    void DrawCards(int cardCount);
    void DrawStartingHand();
    void EmptyManaPool();    
    void AssignDamage(Damage damage);
    void AddManaToManaPool(IManaAmount manaAmount);
    void Mill(int count);
    void TakeMulligan();
    void DealAssignedDamage();
    IEnumerable<Target> GetTargets();
    void MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard();
    void ShuffleLibrary();
    void RemoveDamageFromPermanents();
    void RemoveRegenerationFromPermanents();
    void SetAiVisibility(IPlayer playerOnTheMove);
    void AddModifier(IModifier modifier);
    void RemoveModifier(IModifier modifier);
  }
}