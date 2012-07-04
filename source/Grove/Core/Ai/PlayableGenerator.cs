namespace Grove.Core.Ai
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using Controllers.Results;

  public class PlayableGenerator : IEnumerable<Playable>
  {
    private readonly Game _game;

    private readonly Player _player;

    public PlayableGenerator(Player player, Game game)
    {
      _player = player;
      _game = game;
    }

    public IEnumerator<Playable> GetEnumerator()
    {
      return GetPlayables().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    private IEnumerable<Playable> GetPlayableAbilities()
    {
      foreach (Card card in _player.Battlefield)
      {
        if (card.IsHidden)
          continue;

        List<SpellPrerequisites> abilitiesPrerequisites = card.CanActivateAbilities();

        for (int abilityIndex = 0; abilityIndex < abilitiesPrerequisites.Count; abilityIndex++)
        {
          SpellPrerequisites prerequisites = abilitiesPrerequisites[abilityIndex];

          if (prerequisites.IsManaSource)
            continue;

          if (!prerequisites.CanBeSatisfied)
            continue;


          foreach (Playable playable in GeneratePlayables(card, prerequisites, p => new Ability(card, p, abilityIndex)))
          {
            yield return playable;
          }
        }
      }
    }

    private IEnumerable<Playable> GetPlayableSpells()
    {
      foreach (Card card in _player.Hand)
      {
        if (card.IsHidden)
          continue;

        SpellPrerequisites prerequisites = card.CanCast();

        if (!prerequisites.CanBeSatisfied)
          continue;

        foreach (Playable playable in GeneratePlayables(card, prerequisites, p => new Spell(card, p)))
        {
          yield return playable;
        }

        if (prerequisites.CanCastWithKicker)
        {
          foreach (Playable playable in GeneratePlayables(card, prerequisites,
            p => new Spell(card, p), payKicker: true))
          {
            yield return playable;
          }
        }
      }
    }

    private IEnumerable<Playable> GeneratePlayables(Card card, SpellPrerequisites prerequisites,
                                                    Func<ActivationParameters, Playable> factory, bool payKicker = false)
    {
      var activationGenerator = new ActivationGenerator(
        card,
        prerequisites,
        payKicker: payKicker,
        game: _game);

      int count = 0;
      foreach (ActivationParameters activationParameters in activationGenerator)
      {
        var timingParameters = new TimingParameters(
          _game,
          card,
          activationParameters,
          prerequisites.TargetsSelf);

        if (prerequisites.Timming(timingParameters))
        {
          yield return factory(activationParameters);
          count++;
        }

        if (count >= Search.TargetLimit)
        {
          break;
        }
      }
    }

    private IEnumerable<Playable> GetCyclables()
    {
      foreach (Card card in _player.Hand)
      {
        if (card.IsHidden)
          continue;

        SpellPrerequisites prerequisites = card.CanCycle();

        if (!prerequisites.CanBeSatisfied)
        {
          continue;
        }

        if (!prerequisites.Timming(new TimingParameters(_game, card)))
        {
          continue;
        }

        yield return new Cyclable(card);
      }
    }

    private IEnumerable<Playable> GetPlayables()
    {
      foreach (Playable playable in GetPlayableSpells())
      {
        yield return playable;
      }

      foreach (Playable cyclable in GetCyclables())
      {
        yield return cyclable;
      }

      foreach (Playable playable in GetPlayableAbilities())
      {
        yield return playable;
      }
    }
  }
}