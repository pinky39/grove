using Newtonsoft.Json;
using System.IO;

namespace Grove
{
  public class Settings
  {
    private static Settings _global;
        
    // This should be used only in non game context
    // and should should not be modified!
    public static Settings Readonly
    {
      get
      {
        if (_global == null)
        {
          _global = Settings.Load();
        }
        
        return _global;
      }
    }
    public int BasicLandVersions = 4;
    public static Settings Load()
    {
      const string filename = "settings.json";

      if (File.Exists(filename))
      {
        return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
      }

      return new Settings();
    }
  }
}