using ImageMagick;
using LOTR_CR.CardReaders.Models;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");
    Card card = new("https://sda-src.cgbuilder.fr/images/carte/16/10.jpg");//10 is player, 23 is encounter (debug)
  }
}