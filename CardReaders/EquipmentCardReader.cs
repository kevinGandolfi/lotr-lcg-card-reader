using ImageMagick;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class EquipmentCardReader : CardReader
  {
    public EquipmentCardReader(Card card) : base(card) { }

    public override void GetCardTitle()
    {
      throw new NotImplementedException();
    }
  }
}
