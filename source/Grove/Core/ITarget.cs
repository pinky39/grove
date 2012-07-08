namespace Grove.Core
{
  using System;
  using Controllers.Scenario;
  using Effects;
  using Modifiers;

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
      return target.IsCard() && target.Card().IsPermanent;
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
      return target.Card().HasColor(color);
    }

    public static void DealDamage(this ITarget target, Card damageSource, int amount, bool isCombat = false)
    {
      if (target.IsCard())
      {
        target.Card().DealDamage(damageSource, amount, isCombat);
        return;
      }

      if (target.IsPlayer())
      {
        target.Player().DealDamage(damageSource, amount, isCombat);
        return;
      }
    }

    public static int LifepointsLeft(this ITarget target)
    {
      if (target.IsCard())
      {
        return target.Card().LifepointsLeft;
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

      public bool OfType(string type)
      {
        return false;
      }
    }
  }
}