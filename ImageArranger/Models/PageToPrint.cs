using ImageMagick;

namespace ImageArranger.Models
{
  /// <summary>
  /// Represents the page to print with images.
  /// </summary>
  public class PageToPrint
  {
    private const string FILE_EXTENSION_OUTPUT = ".png";

    private const string _directoryPath = @"..\..\..\PagesToPrint\";

    #region PROPERTIES

    /// <summary>
    /// Page to print.
    /// </summary>
    public MagickImage PageImage { get; init; }

    /// <summary>
    /// List of positions for the images. The boolean value is set to true if the position is available, false otherwise.
    /// </summary>
    public Dictionary<(int x, int y), bool> Positions { get; set; } = [];

    #endregion PROPERTIES

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="imageToPrintWidth">Max width of the images to print.</param>
    /// <param name="imageToPrintHeight">Max height of the images to print.</param>
    /// <param name="margin">Margin between the images.</param>
    public PageToPrint(int imageToPrintWidth, int imageToPrintHeight, int margin)
    {
      int lastXPosition = margin;
      int lastYPosition = margin;
      this.PageImage = new(MagickColors.White, 2480, 3508);
      while (this.HasEnoughHeight(imageToPrintHeight, lastYPosition, margin))
      {
        Positions.Add((lastXPosition, lastYPosition), false);
        if (this.HasEnoughWidth(imageToPrintWidth, lastXPosition, margin))
        {
          lastXPosition += imageToPrintWidth + margin;
        }
        else
        {
          lastXPosition = margin;
          lastYPosition += imageToPrintHeight + margin;
        }
      }
    }

    #region PUBLIC METHODS

    /// <summary>
    /// Saves the page image.
    /// </summary>
    /// <param name="pageNumber">Number of the page.</param>
    public void SavePageImage(int pageNumber)
    {
      this.PageImage.Write($"{_directoryPath}{pageNumber}{FILE_EXTENSION_OUTPUT}");
    }

    /// <summary>
    /// Checks if a page has available space to an image.
    /// </summary>
    /// <returns></returns>
    public bool CanAddToPage()
    {
      return this.Positions.Any(p => p.Value);
    }

    /// <summary>
    /// Adds an image to a page.
    /// </summary>
    /// <param name="imageToPrint"></param>
    public void AddToPage(MagickImage imageToPrint)
    {
      (int x, int y) = this.Positions.First(p => p.Value).Key;
      this.PageImage.Composite(imageToPrint, x, y, CompositeOperator.Over);
      this.Positions[(x, y)] = false;
    }

    #endregion PUBLIC METHODS

    #region PRIVATE METHODS

    private bool HasEnoughWidth(int imageToPrintWidth, int lastXPosition, int margin)
    {
      return lastXPosition - this.PageImage.Width > imageToPrintWidth + margin;
    }

    private bool HasEnoughHeight(int imageToPrintHeight, int lastYPosition, int margin)
    {
      return lastYPosition - this.PageImage.Width > imageToPrintHeight + margin;
    }

    #endregion PRIVATE METHODS
  }
}
