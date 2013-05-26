namespace Grove.Persistance
{
  using System;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;

  [Serializable]
  public class TournamentInfo
  {
    public string Name;
    public int PlayerCount;

    public static TournamentInfo Load(string filename)
    {
      using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
      {
        return (TournamentInfo) new BinaryFormatter().Deserialize(stream);
      }
    }
  }
}