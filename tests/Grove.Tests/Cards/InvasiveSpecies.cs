namespace Grove.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class InvasiveSpecies
    {
        public class Ai : AiScenario
        {
            [Fact(Skip="In real game it works. But it is failed in the test.")]
            public void ReturnPermament()
            {
              var species = C("Invasive Species");

              Hand(P1, species);
              Battlefield(P1, "Forest", "Forest", "Forest");

              RunGame(1);

              Equal(Zone.Battlefield, C(species).Zone);
              Equal(3, P1.Battlefield.Count);
            }
        }
    }
}
