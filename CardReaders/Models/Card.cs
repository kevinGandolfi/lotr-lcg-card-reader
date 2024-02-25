using ImageMagick;

namespace LOTR_CR.CardReaders.Models
{
  public class Card
  {
    #region PROPERTIES

    public CardType Type { get; set; }

    public string Name { get; set; } = string.Empty;

    public MagickImage CardImage { get; set; } = new();

    #endregion PROPERTIES

    #region PUBLIC METHODS

    public void GetCardType()
    {
      throw new NotImplementedException();
    }

    public void GetBottomLabel()
    {
      throw new NotImplementedException();
    }

    #endregion PUBLIC METHODS
  }
}
