namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EnsoulArtifact
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantWithEnsoulArtifact()
      {
        Hand(P1, "Ensoul Artifact");
        Battlefield(P1, "Profane Memento", "Island", "Island");

        RunGame(1);

        Equal(15, P2.Life());
      }

      [Fact]
      public void EnchantEquipmentWithEnsoulArtifact()
      {
        var wall = C("Wall of Frost").IsEnchantedWith("Rogue's Gloves");
        Hand(P1, "Ensoul Artifact");
        Battlefield(P1, wall, "Island", "Island");

        RunGame(1);

        Equal(15, P2.Life());
      }

      [Fact]
      public void EnchantWithEnsoulArtifact2()
      {
        Hand(P1, "Ensoul Artifact");
        Battlefield(P1, "Forest", "Island", "Island", "Rogue's Gloves", "Welkin Tern", "Forest");

        // If an Equipment becomes an artifact creature, it can’t be attached to another creature.
        // AI tries to equip "Rogue's Gloves" itself after casting "Ensoul Artifact", and throws StackOverflow. 
        // If test is ok, then "Ensoul Artifact" works correctly and equipment loses its ability to equp if it becomes creature

        RunGame(1);

        Equal(13, P2.Life());
      }
    }
  }
}