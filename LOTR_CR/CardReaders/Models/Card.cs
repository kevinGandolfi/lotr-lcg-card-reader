using System.Drawing;
using System.Linq;
using ImageMagick;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

namespace LOTR_CR.CardReaders.Models
{
  /// <summary>
  /// Class representing a LOTR:TCG card.
  /// </summary>
  public class Card
  {
    #region CONSTANTS

    private const string _COLOR_SPECIFIC_TO_OBJECTIVE_CARDS = "#898F8F";
    private const string _BOTTOM_LABEL_FILE_NAME = @"..\..\..\bottom_label.png";
    private const string _TESSDATA_LOCATION = @"..\..\..\tessdata";
    private const string _LANGUAGE = "sda-jce";

    #endregion CONSTANTS

    #region FIELDS

    private readonly MagickImage _bottom_label;

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

    /// <summary>
    /// URL of the card.
    /// </summary>
    public string Url { get; set; } = String.Empty;

    public int NumberOfCopies { get; set; }

    #endregion PROPERTIES

    /// <summary>
    /// Constructor that loads a picture.
    /// </summary>
    /// <param name="imageStream"></param>
    public Card(MemoryStream imageStream)
    {
      this.Load(imageStream);
      this._bottom_label = (MagickImage)this.CardImage.Clone();
      this.GetCardType();
      this.GetNumberOfCopies();
    }

    ~Card()
    {
      this.CardImage.Dispose();
      this._bottom_label.Dispose();
    }

    public void WriteBottomLabel(int cardNumber)
    {
      this._bottom_label.Write($@"..\..\..\BottomLabels\{cardNumber}.png");
    }

    #region PRIVATE METHODS

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
      if (this.CardImage.Width > this.CardImage.Height)
      {
        this.Type = CardType.Quest;
        return;
      }

      this.GetBottomLabel();
      string labelText = this.GetLabelText();
      labelText = this.ExtractLetters(labelText);
      this.Type = labelText switch
      {
        string s when s.Contains("héros") 
          || s.Contains("heros")
          || s.Contains("ros")
          => CardType.Hero,
        string s when s.Contains("événement")
          || s.Contains("évén")
          || s.Contains("nement")
          || s.Contains("evé") 
          || s.Contains("évèn") 
          || s.Contains("èvén") 
          || s.Contains("èvè") 
          || s.Contains("eve")
          || s.Contains("even")
          || s.Contains("£v£")
          || s.Contains("ëv")
          => CardType.Event,
        string s when s.Contains("attachemen") 
          || s.Contains("hement") 
          || s.Contains("at") 
          || s.Contains("tach")
          => CardType.Attachment,
        string s when s.Contains("trésor") 
          || s.Contains("tré")
          || s.Contains("sor") 
          => CardType.Treasure,
        string s when s.Contains("lieu") 
          || s.Contains("li€u") 
          => CardType.Location,
        string s when s.Contains("allié")
          || s.Contains("allie")
          || s.Contains("all")
          || s.Contains("lie")
          || s.Contains("li")
          => CardType.Ally,
        string s when s.Contains("traîtrise")
          || s.Contains("traitrise")
          || s.Contains("tris")
          || s.Contains("ise") 
          || s.Contains("aî") 
          || s.Contains("î")
          => CardType.Treachery,
        string s when s.Contains("ennemi") 
          || s.Contains("nnemi")
          || s.Contains("nne") 
          || s.Contains("nn") 
          => CardType.Enemy,
        string s when s.Contains("objectif") => CardType.Objective,
        _ => CardType.Objective,
      };
    }

    private void GetNumberOfCopies()
    {
      this.NumberOfCopies = this.Type switch
      {
        CardType.Hero => 1,
        CardType.Ally => 3,
        CardType.Event => 3,
        CardType.Attachment => 3,
        _ => 1
      };
    }

    /// <summary>
    /// Gets the bottom label of an objective card and stores it in a field.
    /// </summary>
    private void GetBottomLabelOfObjectiveCard()
    {
      this._bottom_label.Crop(0, 43, Gravity.South);
      this._bottom_label.Extent(this._bottom_label.Width, this._bottom_label.Height - 25, Gravity.North);
      this._bottom_label.Extent(140, this._bottom_label.Height, Gravity.Center);
      this._bottom_label.Write(_BOTTOM_LABEL_FILE_NAME); // DEBUG
    }

    /// <summary>
    /// Gets the bottom label of a card and stores it in a field.
    /// </summary>
    private void GetBottomLabel()
    {
      this._bottom_label.Crop(0, 36, Gravity.South);
      this._bottom_label.Extent(this._bottom_label.Width, this._bottom_label.Height - 19, Gravity.North);
      this._bottom_label.Extent(140, this._bottom_label.Height, Gravity.Center);
      this._bottom_label.Grayscale();
      this._bottom_label.MedianFilter(1);
      this._bottom_label.Write(_BOTTOM_LABEL_FILE_NAME); // DEBUG
    }

    /// <summary>
    /// Checks if a color exists in a zone where it may be found in an Objective card.
    /// </summary>
    /// <returns>True if the color exists, false otherwise.</returns>
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
      using (var engine = new TesseractEngine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _TESSDATA_LOCATION)), _LANGUAGE, EngineMode.Default))
      {
        using (var img = Pix.LoadFromFile(_BOTTOM_LABEL_FILE_NAME))
        {
          var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _TESSDATA_LOCATION));
          using (var page = engine.Process(img, PageSegMode.SingleWord))
          {
            labelText = page.GetText();
          }
        }
      }

      return labelText;
    }

    /// <summary>
    /// Removes parasite characters from the tesseract result.
    /// </summary>
    /// <param name="labelText"></param>
    /// <returns></returns>
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
