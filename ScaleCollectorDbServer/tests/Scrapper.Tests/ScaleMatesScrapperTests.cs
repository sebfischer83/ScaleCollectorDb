namespace Scrapper.Tests
{
    public class ScaleMatesScrapperTests
    {
        [Fact]
        public async Task GetKitInformationFromUrlTestAsync()
        {
            ScaleMatesScrapper scaleMates = new ScaleMatesScrapper();
            var result = await scaleMates.GetKitInformationFromUrlAsync("https://www.scalemates.com/kits/takom-8007-ersatz-m7--1403681");


            result.Title.ShouldBe("Ersatz M7");
            result.SubTitle.ShouldBe("2 in 1");
            result.Brand.ShouldBe("Takom");
        }
    }
}