using ImageMagick;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class QuestCardReader : CardReader
  {
    public QuestCardReader(Card card) : base(card) { }

    public override MagickImage GetCardTitle()
    {
      MagickImage cardTitle = (MagickImage)this.CardImage.Clone();
      MagickGeometry titleGeometry = new(103, 4, 443, 51);
      cardTitle.Crop(titleGeometry);
      cardTitle.Format = MagickFormat.Png;
      return cardTitle;
    }

    public override MagickImage GetCardDescription(int height)
    {
      height = 211;
      MagickImage cardDescription = base.GetCardDescription(height);
      cardDescription.Format = MagickFormat.Png;
      MagickImage cardTitle = this.GetCardTitle();
      int posX = 5;
      int posY = 5;
      cardDescription.Composite(cardTitle, posX, posY, CompositeOperator.Over);
      return cardDescription;
    }
  }
}
