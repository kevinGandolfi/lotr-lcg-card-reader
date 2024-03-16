using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class LocationCardReader : CardReader
  {
    public LocationCardReader(Card card) : base(card) { }

    public override void GetCardTitle()
    {
      throw new NotImplementedException();
    }
  }
}
