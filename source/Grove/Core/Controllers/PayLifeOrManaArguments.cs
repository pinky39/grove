namespace Grove.Core.Controllers
{
  public delegate void PayLifeOrManaHandler(PayLifeOrManaArguments args);

  public class PayLifeOrManaArguments
  {
    private readonly object _context;    

    public PayLifeOrManaArguments(bool answer, Player player, Game game, object context)
    {
      Answer = answer;
      Player = player;
      Game = game;
      
      _context = context;
    }

    public Game Game { get; set; }
    public bool Answer { get; private set; }
    public Player Player { get; private set; }

    public T Ctx<T>()
    {
      return (T) _context;
    }
  }
}