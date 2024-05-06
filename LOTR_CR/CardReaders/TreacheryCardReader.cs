using ImageMagick;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders;

public class TreacheryCardReader : CardReader
{
  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="card"></param>
  public TreacheryCardReader(Card card) : base(card) { }

  public override MagickImage GetCardTitle()
  {
    MagickImage cardTitle = (MagickImage)CardImage.Clone();
    MagickGeometry titleGeometry = new(30, 72, 37, 220);
    cardTitle.Crop(titleGeometry);
    using MagickImage mask = new(MagickColors.Transparent, cardTitle.Width, cardTitle.Height);
    mask.Draw(new DrawableRoundRectangle(0, 0, mask.Width - 1, mask.Height - 1, 10, 10));
    cardTitle.Composite(mask, 0, 0, CompositeOperator.CopyAlpha);
    cardTitle.Rotate(90);

    return cardTitle;
  }

  public override MagickImage GetCardDescription(int height)
  {
    height = 290;
    MagickImage cardDescription = base.GetCardDescription(height);
    cardDescription.Format = MagickFormat.Png;
    MagickImage cardTitle = GetCardTitle();
    int posX = (cardDescription.Width - cardTitle.Width) / 2;
    int posY = 0;
    cardDescription.Composite(cardTitle, posX, posY, CompositeOperator.Over);
    return cardDescription;
  }
}
