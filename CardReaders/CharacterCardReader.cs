using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR.CardReaders
{
  /// <summary>
  /// Card reader for character cards.
  /// </summary>
  public class CharacterCardReader : CardReader
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="card"></param>
    public CharacterCardReader(Card card) : base(card) { }

    public override void GetCardTitle()
    {
      throw new NotImplementedException();
    }
  }
}
