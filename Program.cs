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
      try
      {
        MemoryStream imageStream = GetMemoryStreamFromHostedImage();
        Card card = new(imageStream);
        CardReader cardReader = CardReaderFactory.GetCardReader(card);
        images.Add(cardReader.GetCardDescription());
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        break;
      }
    }
  }

  /// <summary>
  /// Builds URL for the first time.
  /// </summary>
  /// <param name="cardNumber"></param>
  /// <returns></returns>
  private static string BuildUrl(short cardNumber)
  {
    StringBuilder stringBuilder = new StringBuilder(BASE_LINK);
    stringBuilder.Append(COLLECTION_NUMBER);
    stringBuilder.Append(_cardNumber);
    stringBuilder.Append(FILE_EXTENSION);
    return stringBuilder.ToString();
  }

  /// <summary>
  /// Gets a MemoryStream from an image hosted online.
  /// </summary>
  /// <returns>The MemoryStream of the image.</returns>
  private static MemoryStream GetMemoryStreamFromHostedImage()
  {
    using (HttpClient httpClient = new())
    {
      if (!IsValidUrl())
      {
        TryQuestUrls();
      }
      if (IsQuestUrl())
      {
        Console.WriteLine(_url);
        TryQuestUrls();
      }
      Console.WriteLine(_url);
      byte[] responseInBytes = httpClient.GetByteArrayAsync(_url).Result;
      _url = GetUrlWithNewCardNumber(++_cardNumber);
      return new MemoryStream(responseInBytes);
    }
  }

  /// <summary>
  /// Checks if the URL is valid.
  /// </summary>
  /// <returns></returns>
  private static bool IsValidUrl()
  {
    using (HttpClient httpClient = new())
    {
      HttpResponseMessage response = httpClient.GetAsync(_url).Result;
      if (response.IsSuccessStatusCode)
      {
        return (response.RequestMessage.RequestUri?.ToString() == _url);
      }
      else
      {
        return false;
      }
    }
  }

  /// <summary>
  /// Tries several "quest" URLs. For example: 23A.jpg, 23B.jpg etc.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  private static MemoryStream TryQuestUrls()
  {
    if (!_url.Contains("A.jpg")
      && !_url.Contains("B.jpg")
      && !_url.Contains("C.jpg"))
    {
      _url = GetQuestUrl('A');
      if (!IsValidUrl())
      {
        throw new Exception("No more files to parse.");
      }
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
      _url = GetUrlWithNewCardNumber(++_cardNumber);
      return GetMemoryStreamFromHostedImage();
    }
  }

  /// <summary>
  /// Gets a URL with a "quest" format.
  /// </summary>
  /// <param name="newLetter"></param>
  /// <returns></returns>
  private static string GetQuestUrl(char newLetter)
  {
    string newFilename = $"{_cardNumber}{newLetter}{FILE_EXTENSION}";
    string directoryPath = _url[..(_url.LastIndexOf('/') + 1)];
    return directoryPath + newFilename;
  }

  /// <summary>
  /// Gets a URL with a card number.
  /// </summary>
  /// <param name="cardNumber"></param>
  /// <returns></returns>
  private static string GetUrlWithNewCardNumber(short cardNumber)
  {
    string newFilename = $"{_cardNumber}{FILE_EXTENSION}";
    string directoryPath = _url[..(_url.LastIndexOf('/') + 1)];
    return directoryPath + newFilename;
  }

  /// <summary>
  /// Checks if the URL has a "quest" format.
  /// </summary>
  /// <returns></returns>
  private static bool IsQuestUrl()
  {
    return (_url.Contains($"A{FILE_EXTENSION}")
      || _url.Contains($"B{FILE_EXTENSION}")
      || _url.Contains($"C{FILE_EXTENSION}"));
  }
}