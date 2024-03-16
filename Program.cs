using System;
using System.Text;
using ImageMagick;
using LOTR_CR;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

class Program
{
  private const string BASE_LINK = "https://sda-src.cgbuilder.fr/images/carte/";
  private const string COLLECTION_NUMBER = "43/";
  private const string FILE_EXTENSION = ".jpg";
  private static short _cardNumber = 1;
  private static string _url = string.Empty;

  static void Main(string[] args)
  {
    List<MagickImage> images = new();

    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");
    _url = BuildUrl(_cardNumber);
    while (true)
    {
      MemoryStream imageStream = GetMemoryStreamFromHostedImage();
      //if (imageStream is null)
      //{
      //  break;
      //}
      Card card = new(imageStream);
      CardReader cardReader = CardReaderFactory.GetCardReader(card);
      images.Add(cardReader.GetCardDescription());
    }
  }

  private static MemoryStream GetMemoryStreamFromHostedImage()
  {
    using (var httpClient = new HttpClient())
    {
      try
      {
        HttpResponseMessage response = httpClient.GetAsync(_url).Result;
        Console.WriteLine(_url);
        if (response.IsSuccessStatusCode)
        {
          if (response.RequestMessage.RequestUri?.ToString() == _url)
          {
            byte[] responseInBytes = httpClient.GetByteArrayAsync(_url).Result;
            if (_url.Contains("A.jpg"))
            {
              _url = GetQuestUrl('B');
            }
            else if (_url.Contains("B.jpg"))
            {
              _url = GetQuestUrl('C');
            }
            else
            {
              _url = BuildUrl(_cardNumber++);
            }
            return new MemoryStream(responseInBytes);
          }
          else
          {
            return null;
          }
        }
        else { return null; }
      }
      catch (Exception)
      {
        if (!_url.Contains("A.jpg")
          && !_url.Contains("B.jpg")
          && !_url.Contains("C.jpg"))
        {
          _url = GetQuestUrl('A');
          return GetMemoryStreamFromHostedImage();
        }
        else if (_url.Contains("A.jpg"))
        {
          _url = GetQuestUrl('B');
          return GetMemoryStreamFromHostedImage();
        }
        else if (_url.Contains("B.jpg"))
        {
          _url = GetQuestUrl('C');
          return GetMemoryStreamFromHostedImage();
        }
        else
        {
          _url = BuildUrl(_cardNumber++);
          return GetMemoryStreamFromHostedImage();
        }
      }
    }
  }

  private static string GetQuestUrl(char newLetter)
  {
    string newFilename = $"{_cardNumber}{newLetter}.jpg";
    string directoryPath = _url[..(_url.LastIndexOf('/') + 1)];
    return directoryPath + newFilename;
  }

  private static string BuildUrl(short cardNumber)
  {
    StringBuilder stringBuilder = new StringBuilder(BASE_LINK);
    stringBuilder.Append(COLLECTION_NUMBER);
    stringBuilder.Append(_cardNumber);
    stringBuilder.Append(FILE_EXTENSION);
    return stringBuilder.ToString();
  }
}