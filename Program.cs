using ImageMagick;
using LOTR_CR;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");
    Card card = new("https://sda-src.cgbuilder.fr/images/carte/43/1.jpg");
    CardReader cardReader = CardReaderFactory.GetCardReader(card);
    //1: hero, 2: ally, 3: event, 5: attachment, 23: treachery, 21: location, 14: enemy, 31: objective, 43/22: treasure
  }
}