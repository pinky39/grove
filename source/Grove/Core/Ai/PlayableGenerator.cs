namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using Controllers.Results;

  public class PlayableGenerator : IEnumerable<Playable>
  {
    private const int Unlimited = 50;
    private readonly Game _game;
    private readonly int _maxTargets;

    private readonly Player _player;
    private readonly Players _players;

    public PlayableGenerator(Player player, Game game, int maxTargets = Unlimited)
    {
      _player = player;
      _players = game.Players;
      _game = game;
      _maxTargets = maxTargets;
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
      foreach (var card in _player.Battlefield)
      {
        if (card.IsHidden)
          continue;

        var abilitiesPrerequisites = card.CanActivateAbilities();

        for (int abilityIndex = 0; abilityIndex < abilitiesPrerequisites.Count; abilityIndex++)
        {
          var prerequisites = abilitiesPrerequisites[abilityIndex];

          if (prerequisites.IsManaSource)
            continue;

          if (!prerequisites.CanBeSatisfied)
            continue;

          var activationGenerator = new ActivationGenerator(
            card, prerequisites, _players, _game.Stack, _maxTargets);

          foreach (var activationParameters in activationGenerator)
          {
            if (!prerequisites.Timming(_game, card, activationParameters))
              continue;

            yield return new Ability(card, activationParameters, abilityIndex);
          }
        }
      }
    }

    private IEnumerable<Playable> GetPlayableSpells()
    {
      foreach (var card in _player.Hand)
      {
        if (card.IsHidden)
          continue;

        var prerequisites = card.CanCast();

        if (!prerequisites.CanBeSatisfied)
          continue;

        var activationGenerator = new ActivationGenerator(
          card, prerequisites, _players, _game.Stack, _maxTargets);

        foreach (var activationParameters in activationGenerator)
        {
          if (!prerequisites.Timming(_game, card, activationParameters))
            continue;

          yield return new Spell(card, activationParameters);
        }
      }
    }

    private IEnumerable<Playable> GetPlayables()
    {
      foreach (var playable in GetPlayableSpells())
      {
        yield return playable;
      }

      foreach (var playable in GetPlayableAbilities())
      {
        yield return playable;
      }
    }
  }
}