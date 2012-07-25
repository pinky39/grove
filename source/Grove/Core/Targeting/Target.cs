namespace Grove.Core.Targeting
{
  using System;
  using Controllers.Scenario;
  using Details.Cards;
  using Details.Cards.Effects;
  using Details.Cards.Modifiers;
  using Details.Mana;
  using Infrastructure;

  [Copyable]
  public class Target : IHashable, IEquatable<Target>
  {
    private readonly Card _card;
    private readonly Effect _effect;
    private readonly IPlayer _player;
    private readonly ScenarioEffect _scenarioEffect;

    public Target(IPlayer player)
    {
      _player = player;
    }

    public Target(Card card)
    {
      _card = card;
    }

    public Target(Effect effect)
    {
      _effect = effect;
    }

    public Target(ScenarioEffect effect)
    {
      _scenarioEffect = effect;
    }

    public int AssignedDamage { get; set; }

    public bool Equals(Target other)
    {
      if (ReferenceEquals(null, other))
        return false;

      if (ReferenceEquals(this, other))
        return true;

      if (IsCard())
        return other._card == _card;

      if (IsPlayer())
        return other._player == _player;

      return other.Effect() == Effect();
    }

    public int CalculateHash(HashCalculator calc)
    {
      if (IsCard())
        return _card.CalculateHash(calc);

      if (IsPlayer())
        return _player.CalculateHash(calc);


      return Effect().CalculateHash(calc);
    }

    public Effect Effect()
    {      
      return _effect ?? _scenarioEffect.Effect();
    }

    public Card Card()
    {
      return _card;
    }

    public IPlayer Player()
    {
      return _player;
    }


    public bool HasColor(ManaColors color)
    {
      return (IsCard() && Card().HasColor(color)) ||
        (IsEffect() && Effect().Source.OwningCard.HasColor(color));
    }

    public void DealDamage(Damage damage)
    {
      if (IsCard())
      {
        Card().DealDamage(damage);
      }

      if (IsPlayer())
      {
        Player().DealDamage(damage);
      }

      throw new InvalidOperationException(
        "Only cards and players can be dealt damage.");
    }

    public int LifepointsLeft()
    {
      if (IsCard())
      {
        return Card().CalculateLifepointsLeft();
      }

      if (IsPlayer())
      {
        return Player().Life;
      }

      throw new InvalidOperationException(
        "Only cards and players have lifepoints.");
    }

    public ITargetType Is()
    {
      return IsCard() ? Card().Is() : new NotCardTargetType();
    }

    public bool IsPlayer()
    {
      return _player != null;
    }

    public bool IsCard()
    {
      return _card != null;
    }

    public bool IsEffect()
    {
      return _effect != null || _scenarioEffect != null;
    }

    public override string ToString()
    {
      if (_card != null)
        return _card.ToString();

      if (_player != null)
        return _player.ToString();

      return Effect().ToString();
    }

    public void AddModifier(IModifier modifier)
    {
      if (IsCard())
        _card.AddModifier(modifier);
      else if (IsPlayer())
        _player.AddModifier(modifier);

      throw new InvalidOperationException(
        "Only cards and players can have modifiers.");
    }

    public void RemoveModifier(IModifier modifier)
    {
      if (IsCard())
        _card.RemoveModifier(modifier);
      else if (IsPlayer())
        _player.RemoveModifier(modifier);

      throw new InvalidOperationException(
        "Only cards and players can have modifiers.");
    }

    public static implicit operator Target(Player player)
    {
      return new Target(player);
    }

    public static implicit operator Target(ScenarioEffect effect)
    {
      return new Target(effect);
    }

    public static implicit operator Target(Effect effect)
    {
      return new Target(effect);
    }

    public static implicit operator Target(Card card)
    {
      return new Target(card);
    }

    public bool IsPermanent()
    {
      return IsCard() && Card().IsPermanent;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      if (ReferenceEquals(this, obj))
        return true;

      if (obj.GetType() != typeof (Target))
        return false;

      return Equals((Target) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        if (IsCard())
          return _card.GetHashCode();

        if (IsPlayer())
          return _player.GetHashCode();

        return Effect().GetHashCode();
      }
    }
  }
}