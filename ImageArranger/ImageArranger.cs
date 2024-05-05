using ImageArranger.Models;
using ImageMagick;

namespace ImageArranger;

public class ImageArranger
{
  public const int MARGIN = 10;

  private int _maxWidth = 0;
  private int _maxHeight = 0;

  public IEnumerable<MagickImage> imagesToPrint = [];

  public ImageArranger()
  {
    this._maxHeight = imagesToPrint.Max(i => i.Height);
    this._maxWidth = imagesToPrint.Max(i => i.Width);
  }

  /// <summary>
  /// Arranges all images onto A4 pages.
  /// </summary>
  public void ArrangeOnPage()
  {
    PageToPrint pageToPrint = new PageToPrint(this._maxWidth, this._maxHeight, MARGIN);
    foreach (MagickImage imageToPrint in imagesToPrint)
    {
      while (ImageArranger.CanAddToPage(pageToPrint))
      {
        KeyValuePair<(int x, int y), bool> keyValuePair = pageToPrint.Positions.First(p => p.Value);
        // Set to false current position
      }
      //pageImage.Composite(imageToPrint, lastXPosition, lastYPosition, CompositeOperator.Over);
    }
  }

  private static bool CanAddToPage(PageToPrint pageToPrint)
  {
    return pageToPrint.Positions.Any(p => p.Value);
  }
}
