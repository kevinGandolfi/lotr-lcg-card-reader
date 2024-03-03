using ImageMagick;
using LOTR_CR.CardReaders.Models;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");
    Card card = new("https://sda-src.cgbuilder.fr/images/carte/16/18.jpg");
    MagickImage bottomLabel = card.GetBottomLabel();
    bottomLabel.Format = MagickFormat.Jpg;
    bottomLabel.Write(@$"D:\\bottom_label.jpg");
    card.GetCardType();
  }
}