namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Net;
  using Media;

  public class DownloadImages : Task
  {
    public override bool Execute(Arguments arguments)
    {
      var setCode = arguments["set"];

      var cardNames = Cards.All.Select(x => x.Name)
        .ToList();

      var directory = Path.Combine(MediaLibrary.BasePath, "cards");
      if(!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }      

      using (var client = new WebClient())
      {
        foreach (var cardName in cardNames)
        {
          var fileName = Path.Combine(directory, cardName) + ".jpg";

          if (File.Exists(fileName))
            continue;

          Console.Write("Downloading image for: {0}...", cardName);

          var url = String.Format(@"http://mtgimage.com/set/{0}/{1}.crop.jpg",
            setCode, cardName);

          try
          {
            client.DownloadFile(url, fileName);
            Console.WriteLine("OK");
          }
          catch
          {
            Console.WriteLine("Failure");
          }          
        }        
      }

      return true;
    }

    public override void Usage()
    {
      Console.WriteLine(
        "usage: ugrove images set=M15\n\nDownloads missing images given set from mtgimage.com and writes them to 'cards' directory.\n\nSupported sets: USG, ULG, UDS, M15, KTK");
    }
  }
}
