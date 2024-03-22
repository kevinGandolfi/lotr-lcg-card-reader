using ImageMagick;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  /// <summary>
  /// Card reader for attachments.
  /// </summary>
  public class AttachmentCardReader : CardReader
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="card"></param>
    public AttachmentCardReader(Card card) : base(card) { }

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
        //cardTitle.Write(@"..\..\..\title.png");//DEBUG
        return cardTitle;
      }
    }

    public override MagickImage GetCardDescription(int height)
    {
      height = 275;
      MagickImage cardDescription = base.GetCardDescription(height);
      cardDescription.Format = MagickFormat.Png;
      MagickImage cardTitle = this.GetCardTitle();
      int posX = (cardDescription.Width - cardTitle.Width) / 2 - 17;
      int posY = 0;
      cardDescription.Composite(cardTitle, posX, posY, CompositeOperator.Over);
      //cardDescription.Write(@"..\..\..\result.png"); // DEBUG
      return cardDescription;
    }
  }
}
