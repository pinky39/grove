namespace Grove.Tests.Unit
{
  using Utils;
  using Xunit;

  public class RatingDownloaderFacts
  {
    //[Fact]
    public void Download1()
    {
      var cardName = "Drana, Kalastria Bloodchief";

      var downloader = new RatingDownloader();
      string rarity;
      var rating = downloader.TryDownloadRating(cardName, out rarity);

      Assert.True(rating > 4);
    }
  }
}