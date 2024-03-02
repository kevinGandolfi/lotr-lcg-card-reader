using ImageMagick;
using Tesseract;

namespace LOTR_CR.CardReaders.Models
{
  public class Card
  {
    #region PROPERTIES

    public CardType Type { get; set; }

    public string Name { get; set; } = string.Empty;

    public MagickImage CardImage { get; set; } = new();

    #endregion PROPERTIES

    public Card(string imageUrl)
    {
      MemoryStream stream = this.GetMemoryStreamFromHostedImage(imageUrl);
      this.Load(stream);
    }

    #region PUBLIC METHODS

    /// <summary>
    /// Gets a stream of a picture hosted on the internet.
    /// </summary>
    /// <param name="imageUrl">URL of the picture.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public MemoryStream GetMemoryStreamFromHostedImage(string imageUrl)
    {
      using (var httpClient = new HttpClient())
      {
        try
        {
          byte[] response = httpClient.GetByteArrayAsync(imageUrl).Result;
          return new MemoryStream(response);
        }
        catch (Exception)
        {
          throw new InvalidOperationException("The image could not be found");
        }
      }
    }

    /// <summary>
    /// Loads an image and stores it into the Image property.
    /// </summary>
    /// <param name="sourcePath">Source path of the image.</param>
    public void Load(MemoryStream imageStream)
    {
      this.CardImage = new MagickImage(imageStream)
      {
        HasAlpha = true
      };
    }

    public void GetCardType()
    {
      //var path = "YourSolutionDirectoryPath";
      //using (var engine = new TesseractEngine(path + Path.DirectorySeparatorChar + "tessdata", "fra", EngineMode.TesseractAndLstm))
      //{
      //  using (var img = Pix.LoadFromFile(sourceFilePath))
      //  {
      //    using (var page = engine.Process(img))
      //    {
      //      var text = page.GetText();
      //      // text variable contains a string with all words found
      //    }
      //  }
      //}
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the bottom part of a card in order to later read it.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public MagickImage GetBottomLabel()
    {
      MagickImage image = this.CardImage;
      image.Crop(0, 45, Gravity.South);
      image.Extent(image.Width, image.Height - 15, Gravity.North);
      return image;
    }

    #endregion PUBLIC METHODS
  }
}
