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
    /// Image of the card title.
    /// </summary>
    public MagickImage? CardTitle { get; set; } = new();

    /// <summary>
    /// Instance of a card.
    /// </summary>
    public Card? Card { get; set; } = card;

    #endregion PROPERTIES

    #region PUBLIC METHODS

    /// <summary>
    /// Gets the card title of the card property.
    /// </summary>
    public abstract void GetCardTitle();

    #endregion PUBLIC METHODS
  }
}