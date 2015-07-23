namespace Grove.AI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class PlayableGenerator : GameObject
  {
    private readonly Player _player;

    private PlayableGenerator() {}

    public PlayableGenerator(Player player, Game game)
    {
      _player = player;
      Game = game;
    }

    private void FindPlayableAbilities(IEnumerable<Card> searchSpace, List<IPlayable> result)
    {
      foreach (var card in searchSpace)
      {
        if (card.IsVisibleToSearchingPlayer == false)
          continue;

        var abilitiesPrerequisites = card.CanActivateAbilities(ignoreManaAbilities: true);

        foreach (var prerequisites in abilitiesPrerequisites)
        {                                        
          var playables = GeneratePlayables(
            prerequisites,
            () => new PlayableAbility());
                    
          if (playables.Count > 0)
          {
            result.AddRange(playables);
          }
        }
      }
    }

    private void FindPlayableSpells(List<IPlayable> result)
    {
      foreach (var card in _player.Hand)
      {
        if (card.IsVisibleToSearchingPlayer == false)
          continue;

        var spellsPrerequisites = card.CanCast();          

        foreach (var prerequisites in spellsPrerequisites)
        {                    
          var playables = GeneratePlayables(
            prerequisites,
            () => new PlayableSpell());
          
          if (playables.Count > 0)
          {
            result.AddRange(playables);
          }
        }
      }
    }

    private List<Playable> GeneratePlayables(ActivationPrerequisites prerequisites, Func<Playable> createPlayable)
    {
      var context = new ActivationContext(_player, prerequisites);
      var result = new List<Playable>();

      var work = prerequisites.Rules.ToList();

      var pass = 1;
      while (work.Count > 0)
      {
        var newWork = new List<MachinePlayRule>();

        foreach (var rule in work)
        {
          var isFinished = rule.Process(pass, context);

          if (context.CancelActivation)
          {
            return result;
          }
            
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

        result.Add(playable);
        return result;        
      }

      foreach (var targetsCombination in context.TargetsCombinations().Take(Ai.CurrentTargetCount))
      {
        var playable = createPlayable();

        playable.Card = prerequisites.Card;
        playable.Index = prerequisites.Index;
        playable.ActivationParameters.Targets = targetsCombination.Targets;
        playable.ActivationParameters.X = targetsCombination.X;
        playable.ActivationParameters.Repeat = targetsCombination.Repeat;

        result.Add(playable);        
      }

      return result;
    }


    public List<IPlayable> GetPlayables()
    {
      var result = new List<IPlayable>();

      FindPlayableSpells(result);

      FindPlayableAbilities(
        searchSpace: _player.Battlefield.Where(x => x.Controller == _player),
        result: result);

      FindPlayableAbilities(
        searchSpace: _player.Opponent.Battlefield.Where(x => x.Controller == _player),
        result: result);

      FindPlayableAbilities(
        searchSpace: _player.Graveyard,
        result: result);

      FindPlayableAbilities(
        searchSpace: _player.Hand,
        result: result);

      return result;
    }

    public List<IPlayable> GetPlayablesForSpell(Card card, 
      bool respectTimingRules,
      bool payManaCost)
    {
      var result = new List<IPlayable>();

      IEnumerable<ActivationPrerequisites> spellsPrerequisites = 
        card.GetCastPrerequisites(payManaCost).Where(x => x.CanBePlayedRegardlessofTime);

      if (respectTimingRules)
      {
        spellsPrerequisites = spellsPrerequisites.Where(x => x.CanBePlayedAtThisTime);
      }         

      foreach (var prerequisites in spellsPrerequisites)
      {                
        var playables = GeneratePlayables(prerequisites,
          () => new PlayableSpell
            {
              ActivationParameters = {PayManaCost = payManaCost}
            });     
                    
        result.AddRange(playables);        
      }

      return result;
    }
  }
}