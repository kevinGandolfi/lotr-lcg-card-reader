using ImageMagick;
using Tesseract;

namespace LOTR_CR.CardReaders.Models
{
  /// <summary>
  /// Class representing a LOTR:TCG card.
  /// </summary>
  public class Card
  {
    #region CONSTANTS

    private const string _COLOR_SPECIFIC_TO_OBJECTIVE_CARDS = "#898F8F";
    private const string _BOTTOM_LABEL_FILE_NAME = @"..\..\..\bottom_label.jpg";
    private const string _TESSDATA_LOCATION = @"..\..\..\tessdata";
    private const string _LANGUAGE = "fra";

    #endregion CONSTANTS

    #region FIELDS

    private MagickImage _bottom_label;

    #endregion FIELDS

    #region PROPERTIES

    /// <summary>
    /// Type of the card.
    /// </summary>
    public CardType Type { get; set; }

    /// <summary>
    /// Name of the card. (Optional)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Image of the card.
    /// </summary>
    public MagickImage CardImage { get; set; } = new();

    #endregion PROPERTIES

    /// <summary>
    /// Constructor that loads a picture hosted online.
    /// </summary>
    /// <param name="imageUrl"></param>
    public Card(string imageUrl)
    {
      MemoryStream stream = Card.GetMemoryStreamFromHostedImage(imageUrl);
      this.Load(stream);
      this._bottom_label = (MagickImage)this.CardImage.Clone();
      this.GetCardType();
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Gets a stream of a picture hosted on the internet.
    /// </summary>
    /// <param name="imageUrl">URL of the picture.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static MemoryStream GetMemoryStreamFromHostedImage(string imageUrl)
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
    private void Load(MemoryStream imageStream)
    {
      this.CardImage = new MagickImage(imageStream)
      {
        HasAlpha = true
      };
    }

    /// <summary>
    /// Gets the card type of the current card.
    /// </summary>
    private void GetCardType()
    {
      if (this.CardImage.Width == 560 && this.CardImage.Height == 394)
      {
        this.Type = CardType.Quest;
        return;
      }

      if (this.IsObjectiveCard())
      {
        this.GetBottomLabelOfObjectiveCard();
      }
      else
      {
        this.GetBottomLabel();
      }
      string labelText = this.GetLabelText();
      labelText = this.ExtractLetters(labelText);

      switch (labelText)
      {
        case "heros":
          this.Type = CardType.Hero;
          break;
        case "allié":
          this.Type = CardType.Ally;
          break;
        case string s when s.Contains("évén"):
          this.Type = CardType.Event;
          break;
        case string s when s.Contains("attachemen"):
          this.Type = CardType.Attachment;
          break;
        case "tresor":
          this.Type = CardType.Treasure;
          break;
        case "li£u":
          this.Type = CardType.Location;
          break;
        case string s when s.Contains("traîtrise"):
          this.Type = CardType.Treachery;
          break;
        case string s when s.Contains("ïçmscnc:"): // This case does not properly work with Tesseract, but it works with lstm
          this.Type = CardType.Objective;
          break;
      }
    }

    /// <summary>
    /// Gets the bottom label of an objective card and stores it in a field.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void GetBottomLabelOfObjectiveCard()
    {
      this._bottom_label.Crop(0, 43, Gravity.South);
      this._bottom_label.Extent(this._bottom_label.Width, this._bottom_label.Height - 25, Gravity.North);
      this._bottom_label.Extent(140, this._bottom_label.Height, Gravity.Center);
      this._bottom_label.Write(_BOTTOM_LABEL_FILE_NAME);
    }

    /// <summary>
    /// Gets the bottom label of a card and stores it in a field.
    /// </summary>
    private void GetBottomLabel()
    {
      this._bottom_label.Crop(0, 38, Gravity.South);
      this._bottom_label.Extent(this._bottom_label.Width, this._bottom_label.Height - 20, Gravity.North);
      this._bottom_label.Extent(140, this._bottom_label.Height, Gravity.Center);
      this._bottom_label.BackgroundColor = MagickColors.White;
      this._bottom_label.Write(_BOTTOM_LABEL_FILE_NAME);
    }

    /// <summary>
    /// Checks if a color exists in a zone where it may be found in an Objective card.
    /// </summary>
    /// <returns>False if the color exists, true otherwise.</returns>
    private bool IsObjectiveCard()
    {
      int startX = 30; // X-coordinate of the starting point of the zone
      int startY = 536; // Y-coordinate of the starting point of the zone
      int width = 4; // Width of the zone
      int height = 5; // Height of the zone

      MagickGeometry geometry = new MagickGeometry(startX, startY, width, height);
      IMagickImage<ushort> image = this.CardImage.Clone();
      image.Crop(geometry);
      MagickColor targetColor = new(_COLOR_SPECIFIC_TO_OBJECTIVE_CARDS);

      using (IPixelCollection<ushort> pixels = image.GetPixels())
      {
        foreach (IPixel<ushort> pixel in pixels)
        {
          if ((MagickColor)pixel.ToColor()! == targetColor)
          {
            return true;
          }
        }
      }

      return false;
    }

    /// <summary>
    /// Gets the label text of a card.
    /// </summary>
    /// <returns></returns>
    private string GetLabelText()
    {
      string labelText = string.Empty;

      using (var engine = new TesseractEngine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _TESSDATA_LOCATION)), _LANGUAGE, EngineMode.TesseractOnly))
      {
        using (var img = Pix.LoadFromFile(_BOTTOM_LABEL_FILE_NAME))
        {
          var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _TESSDATA_LOCATION));
          Console.WriteLine($"tessdata: {path}");
          using (var page = engine.Process(img, PageSegMode.SingleWord))
          {
            labelText = page.GetText();
          }
        }
      }

      return labelText;
    }

    private string ExtractLetters(string labelText)
    {
      labelText = labelText.Replace("_", "");
      labelText = labelText.Replace("-", "");
      labelText = labelText.Replace(".", "");
      labelText = labelText.Replace("”", "");
      labelText = labelText.Replace("—", "");
      labelText = labelText.Trim();
      labelText = labelText.ToLower();
      return labelText;
    }

    #endregion PRIVATE METHODS
  }
}
