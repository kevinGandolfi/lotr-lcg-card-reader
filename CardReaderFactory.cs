using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR
{
  /// <summary>
  /// FActory that builds CardReaders based on the type of card.
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
      switch (card.Type)
      {
        case CardType.Hero:
        case CardType.Ally:
        case CardType.Enemy:
          return new CharacterCardReader(card);
        case CardType.Event:
        case CardType.Treachery:
          return new EventCardReader(card);
        case CardType.Attachment:
        case CardType.Treasure:
          return new TreasureCardReader(card);
        case CardType.Location:
          return new LocationCardReader(card);
        case CardType.Objective:
          return new ObjectiveCardReader(card);
        case CardType.Quest:
          return new QuestCardReader(card);
        default:
          throw new InvalidOperationException("The card does not have any type.");
      }
    }
  }
}
