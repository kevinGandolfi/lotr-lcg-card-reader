using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  public class QuestCardReader : CardReader
  {
    public QuestCardReader(Card card) : base(card) { }

    public override void GetCardTitle()
    {
      throw new NotImplementedException();
    }
  }
}
