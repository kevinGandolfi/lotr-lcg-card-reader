using ImageMagick;

namespace ImageArranger.Models
{
  /// <summary>
  /// Represents the page to print with images.
  /// </summary>
  public class PageToPrint
  {
    /// <summary>
    /// Page to print.
    /// </summary>
    public MagickImage PageImage = new (MagickColors.White, 2480, 3508);

    /// <summary>
    /// List of positions for the images. The boolean value is set to false if the position is not taken, true otherwise.
    /// </summary>
    public Dictionary<(int x, int y), bool> Positions { get; set; } = [];

    public bool IsFull { get; set; } = false;

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

    private bool HasEnoughWidth(int imageToPrintWidth, int lastXPosition, int margin)
    {
      return lastXPosition - this.PageImage.Width > imageToPrintWidth + margin;
    }

    private bool HasEnoughHeight(int imageToPrintHeight, int lastYPosition, int margin)
    {
      return lastYPosition - this.PageImage.Width > imageToPrintHeight + margin;
    }
  }
}
