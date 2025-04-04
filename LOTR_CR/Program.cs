﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using ImageArranger;
using ImageMagick;
using LOTR_CR;
using LOTR_CR.CardReaders;
using LOTR_CR.CardReaders.Models;

class Program
{
  private readonly static string BASE_LINK = "https://sda-src.cgbuilder.fr/images/carte/";
  private readonly static string COLLECTION_NUMBER = "16/";
  private readonly static int COLLECTION_NUMBER_INT = 16;
  private readonly static string FILE_EXTENSION_INPUT = ".jpg";
  private readonly static string FILE_EXTENSION_OUTPUT = ".png";
  private readonly static string DIRECTORY_PATH = @"..\..\..\CardDescriptions\";
  private readonly static string IMAGES_LOCAL_FOLDER = @"..\..\..\..\ImagesToPrint\";
  private readonly static string BOTTOM_LABELS_LOCAL_FOLDER = @"..\..\..\BottomLabels\";
  private readonly static string ALLIES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}allies";
  private readonly static string ATTACHMENTS_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}attachments";
  private readonly static string ENEMIES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}enemies";
  private readonly static string EVENTS_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}events";
  private readonly static string HEROES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}heroes";
  private readonly static string LOCATIONS_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}locations";
  private readonly static string OBJECTIVES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}objectives";
  private readonly static string TREACHERIES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}treacheries";
  private readonly static string TREASURES_LOCAL_FOLDER = $@"{BOTTOM_LABELS_LOCAL_FOLDER}treasures";

  private static string _url = string.Empty;
  private static short _cardNumber = 1;
  private static List<string> _urlsList = [];

  static void Main(string[] args)
  {
    //DEBUG

    //_url = BuildUrl();
    //_url = GetUrlWithNewCardNumber("12");
    //MemoryStream stream = GetMemoryStreamFromHostedImage(_url);
    //Card cardTest = new(stream);
    //CardReader cardReaderTest = CardReaderFactory.GetCardReader(cardTest);
    //cardReaderTest.GetCardDescription().Write(@"..\..\..\result.png");

    //DEBUG

    List<MagickImage> images = [];

    Console.WriteLine("Welcome to the card reader of Lord of the Rings: the Card Game");
    Console.WriteLine("Select a mode: \n" +
      "1 - Import from the website\n" +
      "2 - Import from the local folder\n" +
      "3 - Create ground truth files");
    string? input = Console.ReadLine();
    int mode;
    while (true)
    {
      if (int.TryParse(input, out int modeinput))
      {
        mode = modeinput;
        break;
      }
      else
      {
        continue;
      }
    }

    if (mode == 1)
    {
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
            string regExFileNameNoExtension = @"(\d+).jpg";
            Match match = Regex.Match(url, regExFileNameNoExtension);
            int cardNumber = 0;
            if (match.Success)
            {
              string fileNameWithoutExtension = match.Groups[1].Value;
              if (int.TryParse(fileNameWithoutExtension, out int fileNumber))
              {
                cardNumber = fileNumber;
              }
            }
            if (card.Type == CardType.Enemy
              || card.Type == CardType.Treachery
              || card.Type == CardType.Location
              || card.Type == CardType.Objective)
            {
              card.NumberOfCopies = EncounterCardNumbers.NumbersPerCollection[(COLLECTION_NUMBER_INT, cardNumber)];
            }
            //if(card.Type != CardType.Quest)
            //{
            //  card.WriteBottomLabel(cardNumber + 751); // DEBUG
            //}
            CardReader cardReader = CardReaderFactory.GetCardReader(card);
            MagickImage image = cardReader.GetCardDescription();
            image.AdaptiveResize((int)(image.Width * 1.905), (int)(image.Height * 1.843));
            for (int i = 0; i < card.NumberOfCopies; i++)
            {
              images.Add(image);
              cardReader.GetCardDescription().Write($"{DIRECTORY_PATH}{++a}{FILE_EXTENSION_OUTPUT}");
            }
          }
          PageArranger imageArranger = new(images);
          imageArranger.ArrangeOnPage();

          break;
        }
      }
    }
    else if (mode == 2)
    {
      if (Directory.Exists(IMAGES_LOCAL_FOLDER))
      {
        string[] imagePaths = Directory.GetFiles(IMAGES_LOCAL_FOLDER, "*.png");
        foreach (string imagePath in imagePaths)
        {
          MagickImage image = new(imagePath);
          image.AdaptiveResize((int)(image.Width * 1.905), (int)(image.Height * 1.843));
          images.Add(image);
        }

        PageArranger imageArranger = new(images);
        imageArranger.ArrangeOnPage();
      }
      else
      {
        Console.WriteLine("Folder not found.");
      }
    }
    else if (mode == 3)
    {
      if (Directory.Exists(BOTTOM_LABELS_LOCAL_FOLDER))
      {
        FolderPathGroundTruthValueDictionary folderPathsWithGroundTruths = new()
        {
          { ALLIES_LOCAL_FOLDER, "ALLIÉ" },
          { ATTACHMENTS_LOCAL_FOLDER, "ATTACHEMENT" },
          { ENEMIES_LOCAL_FOLDER, "ENNEMI" },
          { EVENTS_LOCAL_FOLDER, "ÉVÉNEMENT" },
          { HEROES_LOCAL_FOLDER, "HÉROS" },
          { LOCATIONS_LOCAL_FOLDER, "LIEU" },
          { OBJECTIVES_LOCAL_FOLDER, "OBJECTIF" },
          { TREASURES_LOCAL_FOLDER, "TRÉSOR" },
          { TREACHERIES_LOCAL_FOLDER, "TRAÎTRISE" }
        };
        CreateGroundTruthFiles(folderPathsWithGroundTruths);
      }
      else
      {
        Console.WriteLine("Folder not found.");
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
    StringBuilder stringBuilder = new(BASE_LINK);
    stringBuilder.Append(COLLECTION_NUMBER);
    stringBuilder.Append(_cardNumber);
    stringBuilder.Append(FILE_EXTENSION_INPUT);
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
    string newFilename = $"{_cardNumber}{newLetter}{FILE_EXTENSION_INPUT}";
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
    string newFilename = $"{cardNumber}{FILE_EXTENSION_INPUT}";
    string directoryPath = _url[..(_url.LastIndexOf('/') + 1)];
    return directoryPath + newFilename;
  }

  /// <summary>
  /// Checks if the URL has a "quest" format.
  /// </summary>
  /// <returns></returns>
  private static bool IsQuestUrl()
  {
    return _url.Contains($"A{FILE_EXTENSION_INPUT}")
      || _url.Contains($"B{FILE_EXTENSION_INPUT}")
      || _url.Contains($"C{FILE_EXTENSION_INPUT}");
  }

  private static void CreateGroundTruthFiles(FolderPathGroundTruthValueDictionary folderPathsAndGroundTruths)
  {
    foreach (string folderPath in folderPathsAndGroundTruths.Keys)
    {
      if (Directory.Exists(folderPath))
      {
        string[] imagePaths = Directory.GetFiles(folderPath, "*.png");
        foreach (string imagePath in imagePaths)
        {
          string regExFileNameNoExtension = @"(\d+).png";
          Match match = Regex.Match(imagePath, regExFileNameNoExtension);
          if (match.Success)
          {
            string fileNameWithoutExtension = match.Groups[1].Value;
            string filePath = $@"{folderPath}\{fileNameWithoutExtension}.gt.txt";
            string content = folderPathsAndGroundTruths[folderPath];
            File.WriteAllText(filePath, content);
          }
        }
      }
    }
  }

  /// <summary>
  /// Represents a specific dictionary that matches a file path in the bottom labels folder with the value of the ground truth.
  /// </summary>
  public class FolderPathGroundTruthValueDictionary : Dictionary<string, string>
  {
    public new void Add(string folderPath, string groundTruthValue)
    {
      base.Add(folderPath, groundTruthValue);
    }

    public new string this[string folderPath]
    {
      get { return base[folderPath]; }
      set { base[folderPath] = value; }
    }
  }
}