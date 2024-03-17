using ImageMagick;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class EquipmentCardReader : CardReader
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="card"></param>
    public EquipmentCardReader(Card card) : base(card) { }

    public override MagickImage GetCardTitle()
    {
      MagickImage cardTitle = (MagickImage)this.CardImage.Clone();
      MagickGeometry titleGeometry = new(85, 17, 268, 40);
      cardTitle.Crop(titleGeometry);

      using (MagickImage mask = new(MagickColors.Transparent, cardTitle.Width, cardTitle.Height))
      {
        mask.Draw(new DrawableRoundRectangle(0, 0, mask.Width - 1, mask.Height - 1, 10, 10));
        cardTitle.BackgroundColor = MagickColors.Transparent;
        cardTitle.Composite(mask, 0, 0, CompositeOperator.CopyAlpha);
        cardTitle.Format = MagickFormat.Png;
        cardTitle.Write(@"..\..\..\title.png");//DEBUG
        return cardTitle;
      }
    }

    public override MagickImage GetCardDescription()
    {
      MagickImage cardDescription = base.GetCardDescription();
      cardDescription.Format = MagickFormat.Png;
      MagickImage cardTitle = this.GetCardTitle();
      int resultWidth = Math.Max(cardDescription.Width, cardTitle.Width);
      int resultHeight = cardDescription.Height + cardTitle.Height;
      MagickImage resultImage = new MagickImage(MagickColors.Transparent, resultWidth, resultHeight);
      int posX = (cardDescription.Width - cardTitle.Width) / 2;
      int posY = 0;
      resultImage.Composite(cardTitle, posX, posY, CompositeOperator.Over);
      resultImage.Composite(cardDescription, 0, cardTitle.Height, CompositeOperator.Over);
      //resultImage.Write(@"..\..\..\description.png");
      return resultImage;
    }
  }
}
