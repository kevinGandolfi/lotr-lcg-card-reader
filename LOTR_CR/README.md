# lotr-lcg-card-reader
Extracts text from cards of The Lord of the Rings: the card game

The goal of the program is to extract descriptions (and titles) from cards of the game "The Lord of the Rings: the Card Game".
I specifically made it because I got my hands on German cards, and I don't happen to understand this language. So I wanted to insert the localized
descriptions inside the sleeved cards. Since each card is different, each process will be a bit different. For example, attachments have their title
at the top, events have it on the left side, and allies at the top of the description.
The program uses OCD (Tesseract engine) to determine the type of card, and then extracts its description. Depending on the card, the title might be extracted as
well, and placed right on top of the description.

FUTURE IMPROVEMENTS:
- Determine if a card is an objective card (the label is located at a different place)
- Improve text recognition by Tesseract
- Call each valid URL only once (it is called twice)