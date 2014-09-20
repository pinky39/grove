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
      var rating = downloader.TryDownloadRating(cardName);

      Assert.True(rating > 4);
    }
  }
}