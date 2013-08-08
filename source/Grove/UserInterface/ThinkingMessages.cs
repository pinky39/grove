namespace Grove.UserInterface
{
  using Infrastructure;

  public static class ThinkingMessages
  {
    private static readonly string[] Messages = {
      "The last time I tried this the monkey didn't survive.",
      "Testing data on Timmy... ... ... We're going to need another Timmy.",
      "Would you prefer chicken, steak, or tofu?",
      "Pay no attention to the man behind the curtain.",
      "Spinning up the hamster...",
      "Please wait and dream of faster computers.",
      "Go ahead -- hold your breath",
      "You're not in Kansas any more",
      "Are you pondering what I'm pondering?",
      "Don't think of purple hippos.",
      "Why don't you order a sandwich?",
      "Please wait, while the satellite moves into position.",
      "Please wait, the bits are flowing slowly today.",
      "Dig on the 'X' for buried treasure... ARRR!.",
      "Must go faster, must go faster.",
      "All the relevant elves are on break. Please wait.",
      "You shouldn't have done that.",
      "Just stalling to simulate activity.",
      "I know this is painful to watch, but I have to load this.",
      "Hello!!! Why did you press that button?!",
      "Waking up the AI..."
    };

    public static string GetRandom()
    {
      return Messages[RandomEx.Next(Messages.Length)];
    }
  }
}