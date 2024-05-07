using ImageArranger.Models;
using ImageMagick;

namespace ImageArranger;

/// <summary>
/// Arranges images to print on a page.
/// </summary>
public class PageArranger
{
  #region CONSTANTS

  private const int MARGIN = 30;

  #endregion CONSTANTS

  #region FIELDS

  private readonly int _maxWidth;
  private readonly int _maxHeight;
  private readonly IEnumerable<MagickImage> _imagesToPrint = [];

  #endregion FIELDS

  #region CONSTRUCTORS

  /// <summary>
  /// Constructor.
  /// </summary>
  public PageArranger(IEnumerable<MagickImage> imagesToPrint)
  {
    this._imagesToPrint = imagesToPrint;
    this._maxHeight = _imagesToPrint.Max(i => i.Height);
    this._maxWidth = _imagesToPrint.Max(i => i.Width);
  }

  #endregion CONSTRUCTORS

  #region PUBLIC METHODS

  /// <summary>
  /// Arranges all images onto (A4) pages.
  /// </summary>
  public void ArrangeOnPage()
  {
    PageToPrint pageToPrint = new(this._maxWidth, this._maxHeight, MARGIN);
    int pageNumber = 1;
    foreach (MagickImage imageToPrint in _imagesToPrint)
    {
      if (pageToPrint.CanAddToPage())
      {
        pageToPrint.AddToPage(imageToPrint);
      }
      else
      {
        pageToPrint.SavePageImage(pageNumber);
        pageNumber++;
        pageToPrint.PageImage.Dispose();
        pageToPrint = new(this._maxWidth, this._maxHeight, MARGIN);
        pageToPrint.AddToPage(imageToPrint);
      }
    }
  }

  #endregion PUBLIC METHODS
}
