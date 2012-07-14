MAGICGROVE v 1.4
=====================================
Magicgrove is a 2 player fantasy card game. You and computer are each given a deck of cards which you use as best as you can to win the game. The most common way to win is to reduce your opponent life to 0.

1. Installation
=====================================
Unzip to a folder of your choice. Run grove.exe to play the game.
.Net Framework 4.0 is required to play the game.

2. Gameplay
=====================================
Currently only games with preconstructed decks can be played. You can construct your own decks as long as you use only the cards listed in cards.txt file. For a deck to be visible to the game, it must reside inside .\media\deck folder and the filename must have .dec extension.

Tapping lands is not needed in most of the cases, just click the spell and the mana will be payed automaticly. If you want to have absolute control, you can tap the lands manualy though.

Shortcuts
--------------------------------------------------------------------------
Spacebar 		- moves to next step (passes priority)
Alt+q 			- diplays quit the game screen.
Ctrl+d 			- generates a test scenario.

3. Themes
=====================================
Game visuals can be tweaked by modifying the contents of the media folder.

If you wish you can add your own card pictures to the /media/cards folder. Picture size has to be 410x326 pixels, their names must match coresponding card names (with illegal characters excluded). Basic lands have 4 versions eg. forest1.jpg, ... , forest4.jpg.

Additional battlefield backgrounds can be added to /media/images folder. The filename must start with the word 'battlefield' e.g 'battlefield-1.jpg', 'battlefield-2.png'... At the start of the game a random battlefield background is chosen amongst all available.

Other images can also be replaced, as long as they have the same size as the originals.

4. Source code
=====================================
The source code is available at http://code.google.com/p/magicgrove/

5. License
=====================================
The project source code is licensed under MIT License.

The project uses the following libraries with coresponding licenses:

- log4net library (Apache Software Foundation, licensed under Apache License Version 2.0.)
- caliburn.micro library (Blue Spire Consulting, Inc., licensed under MIT License)
- castle.windsor (Castle Project, licensed under Apache License Version 2.0.)
- xunit (Microsoft Public License)
- psake (by James Kovacs)
- costura (http://code.google.com/p/costura/, MIT Licence)