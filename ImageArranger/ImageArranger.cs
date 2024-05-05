using ImageArranger.Models;
using ImageMagick;

namespace ImageArranger;

public class ImageArranger
{
  public const int MARGIN = 10;

  private int _maxWidth = 0;
  private int _maxHeight = 0;

  public IEnumerable<MagickImage> imagesToPrint = [];
  public List<PageToPrint> PagesToPrint { get; set; } = [];

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
    foreach (MagickImage imageToPrint in imagesToPrint)
    {
      this.PagesToPrint.Add(new PageToPrint(this._maxWidth, this._maxHeight, MARGIN));
      //pageImage.Composite(imageToPrint, lastXPosition, lastYPosition, CompositeOperator.Over);
    }
  }
}
