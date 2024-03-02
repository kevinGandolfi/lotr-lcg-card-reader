using CardReaders;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

namespace LOTR_CR
{
  public class CardReaderFactory
  {
    public static CardReader GetCardReader(Card card)
    {
      switch (card.Type)
      {
        case CardType.Hero:
        case CardType.Ally:
        case CardType.Enemy:
          return new CharacterCardReader();
        case CardType.Event:
        case CardType.Treachery:
          return new EventCardReader();
        case CardType.Attachment:
        case CardType.Treasure:
          return new EquipmentCardReader();
        case CardType.Location:
          return new LocationCardReader();
        case CardType.Objective:
          return new ObjectiveCardReader();
        case CardType.Quest:
          return new QuestCardReader();
        default:
          throw new InvalidOperationException("The card does not have any type.");
      }
    }
  }
}
