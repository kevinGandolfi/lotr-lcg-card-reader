using System.Text;
using ImageArranger;
using ImageMagick;
using LOTR_CR;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

class Program
{
  private const string BASE_LINK = "https://sda-src.cgbuilder.fr/images/carte/";
  private const string COLLECTION_NUMBER = "43/";
  private const string FILE_EXTENSION = ".jpg";
  private const string FILE_EXTENSION_OUTPUT = ".png";
  private static short _cardNumber = 1;
  private static string _url = string.Empty;
  private static string _directoryPath = @"..\..\..\CardDescriptions\";
  private static List<string> _urlsList = [];

  static void Main(string[] args)
  {
    List<MagickImage> images = [];

    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");

    //DEBUG

    // _url = BuildUrl();
    // _url = GetUrlWithNewCardNumber("82");//18 Event, 13 Attachment, 63 Enemy, 70 Treachery, 50 objective, 40 location, 29A quest
    // MemoryStream stream = GetMemoryStreamFromHostedImage(_url);
    // Card cardTest = new(stream);
    // CardReader cardReaderTest = CardReaderFactory.GetCardReader(cardTest);
    // cardReaderTest.GetCardDescription().Write(@"..\..\..\result.png");

    //DEBUG

    _url = BuildUrl();
    int a = 1;
    while (true)
    {
      try
      {
        SetUrlsList();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        foreach (string url in _urlsList)
        {
          Console.WriteLine(url);
          MemoryStream imageStream = GetMemoryStreamFromHostedImage(url);
          Card card = new(imageStream);
          CardReader cardReader = CardReaderFactory.GetCardReader(card);
          MagickImage image = cardReader.GetCardDescription();
          image.AdaptiveResize((int)(image.Width * 1.905), (int)(image.Height * 1.843));
          for (int i = 0; i < card.NumberOfCopies; i++)
          {
            images.Add(image);
            cardReader.GetCardDescription().Write($"{_directoryPath}{++a}{FILE_EXTENSION_OUTPUT}");
          }
        }
        PageArranger imageArranger = new(images);
        imageArranger.ArrangeOnPage();

        break;
      }
    }
  }

  /// <summary>
  /// Builds URL for the first time.
  /// </summary>
  /// <param name="cardNumber"></param>
  /// <returns></returns>
  private static string BuildUrl()
  {
    StringBuilder stringBuilder = new (BASE_LINK);
    stringBuilder.Append(COLLECTION_NUMBER);
    stringBuilder.Append(_cardNumber);
    stringBuilder.Append(FILE_EXTENSION);
    return stringBuilder.ToString();
  }

  /// <summary>
  /// Gets a MemoryStream from an image hosted online.
  /// </summary>
  /// <returns>The MemoryStream of the image.</returns>
  private static void SetUrlsList()
  {
    if (!IsValidUrl())
    {
      TryQuestUrls();
    }
    if (IsQuestUrl())
    {
      _urlsList.Add(_url);
      TryQuestUrls();
    }
    _urlsList.Add(_url);
    ++_cardNumber;
    _url = GetUrlWithNewCardNumber(_cardNumber.ToString());
  }

  private static MemoryStream GetMemoryStreamFromHostedImage(string url)
  {
    using (HttpClient httpClient = new())
    {
      byte[] responseInBytes = httpClient.GetByteArrayAsync(url).Result;
      return new MemoryStream(responseInBytes);
    }
  }

  /// <summary>
  /// Checks if the URL is valid.
  /// </summary>
  /// <returns></returns>
  private static bool IsValidUrl()
  {
    using HttpClient httpClient = new();
    HttpResponseMessage response = httpClient.GetAsync(_url).Result;
    if (response.IsSuccessStatusCode)
    {
        return response.RequestMessage!.RequestUri?.ToString() == _url;
    }
    else
    {
        return false;
    }
  }

  /// <summary>
  /// Tries several "quest" URLs. For example: 23A.jpg, 23B.jpg etc.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  private static void TryQuestUrls()
  {
    if (!_url.Contains("A.jpg")
      && !_url.Contains("B.jpg"))
    {
      _url = GetQuestUrl('A');
      if (!IsValidUrl())
      {
        throw new Exception("No more files to parse.");
      }
      SetUrlsList();
    }
    else if (_url.Contains("A.jpg"))
    {
      _url = GetQuestUrl('B');
      SetUrlsList();
    }
    else if (_url.Contains("B.jpg"))
    {
      ++_cardNumber;
      _url = GetUrlWithNewCardNumber(_cardNumber.ToString());
      SetUrlsList();
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
  private static string GetUrlWithNewCardNumber(string cardNumber)
  {
    string newFilename = $"{cardNumber}{FILE_EXTENSION}";
    string directoryPath = _url[..(_url.LastIndexOf('/') + 1)];
    return directoryPath + newFilename;
  }

  /// <summary>
  /// Checks if the URL has a "quest" format.
  /// </summary>
  /// <returns></returns>
  private static bool IsQuestUrl()
  {
    return _url.Contains($"A{FILE_EXTENSION}")
      || _url.Contains($"B{FILE_EXTENSION}")
      || _url.Contains($"C{FILE_EXTENSION}");
  }
}