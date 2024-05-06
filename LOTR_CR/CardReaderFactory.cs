using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR
{
  /// <summary>
  /// Factory that builds CardReaders based on the type of card.
  /// </summary>
  public class CardReaderFactory
  {
    /// <summary>
    /// Builds the card reader instance.
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static CardReader GetCardReader(Card card)
    {
      return card.Type switch
      {
          CardType.Hero or CardType.Ally or CardType.Enemy => new CharacterCardReader(card),
          CardType.Event => new EventCardReader(card),
          CardType.Treachery => new TreacheryCardReader(card),
          CardType.Attachment => new AttachmentCardReader(card),
          CardType.Treasure => new TreasureCardReader(card),
          CardType.Location => new LocationCardReader(card),
          CardType.Objective => new ObjectiveCardReader(card),
          CardType.Quest => new QuestCardReader(card),
          _ => throw new InvalidOperationException("The card does not have any type."),
      };
    }
  }
}
