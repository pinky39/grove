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

    private void FindPlayableAbilities(IEnumerable<Card> cards, List<IPlayable> allPlayables)
    {
      foreach (var card in cards)
      {
        if (card.IsVisibleToSearchingPlayer == false)
          continue;

        var abilitiesPrerequisites = card.CanActivateAbilities(ignoreManaAbilities: true);

        foreach (var prerequisites in abilitiesPrerequisites)
        {
          var playables = GeneratePlayables(prerequisites, () => new PlayableAbility())
            .ToList();

          // lazy cost evaluation
          if (playables.Count > 0 && prerequisites.CanPay.Value)
          {
            allPlayables.AddRange(playables);
          }
        }
      }
    }

    private void FindPlayableSpells(IEnumerable<Card> cards, List<IPlayable> allPlayables)
    {            
      foreach (var card in cards)
      {
        if (card.IsVisibleToSearchingPlayer == false)
          continue;
        
        var spellsPrerequisites = card.CanCast();

        foreach (var prerequisites in spellsPrerequisites)
        {
          var playables = GeneratePlayables(prerequisites, () => new PlayableSpell())
            .ToList();

          // lazy cost evaluation
          if (playables.Count > 0 && prerequisites.CanPay.Value)
          {
            allPlayables.AddRange(playables);
          }
        }
      }
    }

    private IEnumerable<Playable> GeneratePlayables(ActivationPrerequisites prerequisites, Func<Playable> createPlayable)
    {
      var context = new ActivationContext(prerequisites);

      var work = prerequisites.Rules.ToList();
      
      var pass = 1;
      while (work.Count > 0)
      {
        var newWork = new List<MachinePlayRule>();
        
        foreach (var rule in work)
        {
          var isFinished = rule.Process(pass, context);
          
          if (context.CancelActivation)
            yield break;

          if (!isFinished)
          {
            newWork.Add(rule);
          }
            
        }
        pass++;
        work = newWork;
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


    public List<IPlayable> GetPlayables()
    {
      var all = new List<IPlayable>();

      FindPlayableSpells(_player.Hand, all);
      FindPlayableAbilities(_player.Battlefield.Concat(_player.Hand), all);

      return all;
    }
  }
}