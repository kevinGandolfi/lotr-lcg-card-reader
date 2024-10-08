﻿using ImageMagick;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class EventCardReader : CardReader
  {
    public EventCardReader(Card card) : base(card) { }

    public override MagickImage GetCardTitle()
    {
      MagickImage cardTitle = (MagickImage)this.CardImage.Clone();
      MagickGeometry titleGeometry = new(30, 72, 37, 194);
      cardTitle.Crop(titleGeometry);
      using MagickImage mask = new(MagickColors.Transparent, cardTitle.Width, cardTitle.Height);
      mask.Draw(new DrawableRoundRectangle(0, 0, mask.Width - 1, mask.Height - 1, 10, 10));
      cardTitle.Composite(mask, 0, 0, CompositeOperator.CopyAlpha);
      cardTitle.Rotate(90);

      return cardTitle;
    }

    public override MagickImage GetCardDescription(int height)
    {
      this.GetCardTitle().Write(@"..\..\..\title.png");
      height = 290;
      MagickImage cardDescription = base.GetCardDescription(height);
      cardDescription.Format = MagickFormat.Png;
      MagickImage cardTitle = this.GetCardTitle();
      int posX = (cardDescription.Width - cardTitle.Width) / 2;
      int posY = 0;
      cardDescription.Composite(cardTitle, posX, posY, CompositeOperator.Over);
      return cardDescription;
    }
  }
}
