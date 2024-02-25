using ImageMagick;

namespace CardReaders
{
  public abstract class CardReader
  {
    #region PROPERTIES

    public MagickImage? Image = null;

    #endregion PROPERTIES

    #region PUBLIC METHODS

    public async Task<MemoryStream> GetMemoryStreamFromHostedImage(string imageUrl)
    {
      using (var httpClient = new HttpClient())
      {
        try
        {
          var response = await httpClient.GetByteArrayAsync(imageUrl);
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
      this.Image = new MagickImage(imageStream)
      {
        HasAlpha = true
      };
    }


    #endregion PUBLIC METHODS
  }
}