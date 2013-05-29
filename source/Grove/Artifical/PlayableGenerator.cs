namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Decisions.Results;
  using Gameplay.Misc;

  public class PlayableGenerator : GameObject
  {
    private readonly Player _player;

    private PlayableGenerator() {}

    public PlayableGenerator(Player player, Game game)
    {
      _player = player;
      Game = game;
    }

    private void FindPlayableAbilities(IEnumerable<Card> cards, List<Playable> allPlayables)
    {
      foreach (var card in cards)
      {
        if (card.IsVisible == false)
          continue;

        var abilitiesPrerequisites = card.CanActivateAbilities(ignoreManaAbilities: true);

        foreach (var prerequisites in abilitiesPrerequisites)
        {
          var playables = GeneratePlayables(prerequisites, () => new PlayableAbility());

          allPlayables.AddRange(playables);
        }
      }
    }

    private void FindPlayableSpells(IEnumerable<Card> cards, List<Playable> allPlayables)
    {
      foreach (var card in cards)
      {
        if (!card.IsVisible)
          continue;

        var spellsPrerequisites = card.CanCast();

        foreach (var prerequisites in spellsPrerequisites)
        {
          var playables = GeneratePlayables(prerequisites, () => new PlayableSpell());

          allPlayables.AddRange(playables);
        }
      }
    }

    private IEnumerable<Playable> GeneratePlayables(ActivationPrerequisites prerequisites, Func<Playable> createPlayable)
    {
      var context = new ActivationContext(prerequisites);

      foreach (var rule in prerequisites.Rules)
      {
        rule.Process(context);

        if (context.CancelActivation)
          yield break;
      }


      if (context.HasTargets == false)
      {
        var playable = createPlayable();

        playable.Card = prerequisites.Card;
        playable.Index = prerequisites.Index;
        playable.ActivationParameters.X = context.X;
        playable.ActivationParameters.Repeat = context.Repeat;

        yield return playable;
        yield break;
      }

      foreach (var targetsCombination in context.TargetsCombinations().Take(Ai.Parameters.TargetCount))
      {
        var playable = createPlayable();

        playable.Card = prerequisites.Card;
        playable.Index = prerequisites.Index;
        playable.ActivationParameters.Targets = targetsCombination.Targets;
        playable.ActivationParameters.X = targetsCombination.X;
        playable.ActivationParameters.Repeat = targetsCombination.Repeat;

        yield return playable;
      }

      yield break;
    }


    public List<Playable> GetPlayables()
    {
      var all = new List<Playable>();

      FindPlayableSpells(_player.Hand, all);
      FindPlayableAbilities(_player.Battlefield.Concat(_player.Hand), all);

      return all;
    }
  }
}