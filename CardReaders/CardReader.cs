using ImageMagick;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="card"></param>
  public abstract class CardReader(Card card)
  {
    #region PROPERTIES

    /// <summary>
    /// Instance of a card.
    /// </summary>
    public MagickImage CardImage { get; set; } = card.CardImage;

    #endregion PROPERTIES

    #region PUBLIC METHODS

    /// <summary>
    /// Gets the card title of the card property.
    /// </summary>
    public abstract MagickImage GetCardTitle();

    /// <summary>
    /// Gets the card description of the card property.
    /// </summary>
    public virtual MagickImage GetCardDescription(int height = 235)
    {
      MagickImage cardDescription = (MagickImage)card.CardImage.Clone();
      cardDescription.Crop(0, height, ImageMagick.Gravity.South);
      return cardDescription;
    }

    #endregion PUBLIC METHODS
  }
}