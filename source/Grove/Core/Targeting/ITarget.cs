namespace Grove.Core.Targeting
{
  using System;
  using Controllers.Scenario;
  using Details.Cards;
  using Details.Cards.Effects;
  using Details.Cards.Modifiers;
  using Details.Mana;
  using Infrastructure;
  using Zones;

  public interface ITarget : IHashable
  {
    void AddModifier(IModifier modifier);
    void RemoveModifier(IModifier modifier);
  }

  public static class TargetEx
  {
    public static Card Card(this ITarget target)
    {
      return target as Card;
    }

    public static Player Player(this ITarget target)
    {
      return target as Player;
    }

    public static Effect Effect(this ITarget target)
    {
      var effect = target as Effect;

      if (effect != null)
        return effect;

      var lazyEffect = target as LazyEffect;

      return lazyEffect != null ? lazyEffect.Effect() : null;
    }

    public static bool IsPermanent(this ITarget target)
    {
      return target.IsCard() && target.Card().Zone == Zone.Battlefield;
    }

    public static bool IsCard(this ITarget target)
    {
      return target is Card;
    }

    public static bool IsPlayer(this ITarget target)
    {
      return target is Player;
    }

    public static bool IsEffect(this ITarget target)
    {
      return target is Effect || target is LazyEffect;
    }

    public static bool HasColor(this ITarget target, ManaColors color)
    {
      return (target.IsCard() && target.Card().HasColor(color)) ||
        (target.IsEffect() && target.Effect().Source.OwningCard.HasColor(color));
    }

    public static void DealDamage(this ITarget target, Damage damage)
    {
      if (target.IsCard())
      {
        target.Card().DealDamage(damage);
      }

      if (target.IsPlayer())
      {
        target.Player().DealDamage(damage);
      }
    }

    public static int LifepointsLeft(this ITarget target)
    {
      if (target.IsCard())
      {
        return target.Card().CalculateLifepointsLeft();
      }

      if (target.IsPlayer())
      {
        return target.Player().Life;
      }

      throw new InvalidOperationException("Not a valid target");
    }

    public static ITargetType Is(this ITarget target)
    {
      if (target.IsCard())
      {
        return target.Card().Is();
      }

      return new NotCardTargetType();
    }

    private class NotCardTargetType : ITargetType
    {
      public bool Artifact { get { return false; } }

      public bool Attachment { get { return false; } }
      public bool BasicLand { get { return false; } }
      public bool Creature { get { return false; } }
      public bool Enchantment { get { return false; } }
      public bool Equipment { get { return false; } }
      public bool Instant { get { return false; } }
      public bool Land { get { return false; } }
      public bool Legendary { get { return false; } }
      public bool Sorcery { get { return false; } }
      public bool Token { get { return false; } }
      public bool Aura { get { return false; } }

      public bool OfType(string type)
      {
        return false;
      }
    }
  }
}