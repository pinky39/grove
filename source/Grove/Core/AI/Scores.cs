namespace Grove.AI
{
  using System.Collections.Generic;

  public static class Scores
  {
    public const int LandInHandCost = 90;

    public static readonly Dictionary<int, int> LandsOnBattlefieldToLandScore = new Dictionary<int, int>
      {
        {1, 600},
        {2, 500},
        {3, 450},
        {4, 425},
        {5, 400},
        {6, 375},
      };

    public static readonly Dictionary<int, int> LifeToScore = new Dictionary<int, int>
      {
        {20, 0},
        {19, -40},
        {18, -80},
        {17, -120},
        {16, -160},
        {15, -200},
        {14, -250},
        {13, -300},
        {12, -350},
        {11, -420},
        {10, -490},
        {9, -590},
        {8, -690},
        {7, -790},
        {6, -850},
        {5, -1000},
        {4, -1150},
        {3, -1400},
        {2, -1750},
        {1, -2300},
      };

    public static readonly Dictionary<int, int> ManaCostToScore = new Dictionary<int, int>
      {
        {0, 140},
        {1, 150},
        {2, 200},
        {3, 250},
        {4, 300},
        {5, 350},
        {6, 400},
        {7, 450},
      };

    public static readonly Dictionary<int, int> ManaCostToScoreEcho = new Dictionary<int, int>
      {
        {0, 140},
        {1, 190},
        {2, 240},
        {3, 290},
        {4, 340},
        {5, 390},
        {6, 440},
        {7, 490},
      };

    public static readonly Dictionary<int, int> PowerToughnessToScore = new Dictionary<int, int>
      {
        {0, 0},
        {1, 140},
        {2, 160},
        {3, 180},
        {4, 200},
        {5, 220},
        {6, 240},
        {7, 260},
        {8, 280},
        {9, 300},
        {10, 320},
      };
  }
}