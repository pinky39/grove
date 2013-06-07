MAGICGROVE v 1.9
=====================================
Magicgrove is a 2 player fantasy card game. You and computer are each given a deck of cards which you use as best as you can to win the game. The most common way to win is to reduce your opponent life to 0.

1. Requirements & Installation
=====================================
A modern computer with Windows operating system is required. Although the game can run on single core machines a multple core machine is recommended. .Net Framework 4.0 is required to play the game.

To install, unzip to a folder of your choice. Run grove.exe to play the game.

2. Game types
=====================================

Single match     
-------------------------------------
You select a preconstructed deck for you and your opponent. Whoever wins 2 games first wins the match.

Single random match     
-------------------------------------
You and your opponent are given 2 random decks with same ratings. Whoever wins 2 games first wins the match.

Sealed tournament
-------------------------------------
You are given a pack of cards, which you use to build your deck. You play a swiss style tournament against up to 200 opponents.

3. Gameplay
=====================================

Saving & Loading
-------------------------------------
Every game can be saved via the in game menu (alt-q).
Games can be loaded via the Load saved game screen.

Tapping lands for mana
-------------------------------------
Tapping lands is not needed in most of the cases, just click the spell and the mana will be payed automaticly. If you want to have absolute control, you can tap the lands manualy though.

Priority passing
-------------------------------------
Priority is by default automaticly passed for each step, except for First main (active turn), Second main (active turn) and Declare blockers (active & passive turns).

You can change defaults by clicking on coresponding step's button. A button has 4 states:

Transparent - priority is automaticly passed on active & passive turns.
Green       - priority is automaticly passed on passive turns.
Yellow      - priority is automaticly passed on active turn.
Red         - priority is never automaticly passed.

When an opponent plays a spell auto priority passing is disabled.

Shortcuts
-------------------------------------
Spacebar    - moves to next step (passes priority)
Alt+q       - diplays in game menu screen.
F11         - toggle between window and fullscreen mode

4. Themes
=====================================
Game visuals can be tweaked by modifying the contents of the media folder.

If you wish you can add your own card pictures to the /media/cards folder. Picture size has to be 410x326 pixels, their names must match coresponding card names. Basic lands have 4 versions eg. forest1.jpg, ... , forest4.jpg.

Additional battlefield backgrounds can be added to /media/images folder. The filename must start with the word 'battlefield' e.g 'battlefield-1.jpg', 'battlefield-2.png'... At the start of the game a random battlefield background is chosen amongst all available.

Other images can also be replaced, as long as they have the same size as the originals.

5. Adding cards
=====================================
Currently new cards can only be added by modifying the program source code.

6. Source code
=====================================
The source code is available at http://code.google.com/p/magicgrove/

7. License
=====================================
The project source code is licensed under GPL-3 License.

The project uses the following libraries with coresponding licenses:

- log4net library (Apache Software Foundation, licensed under Apache License Version 2.0.)
- caliburn.micro library (Blue Spire Consulting, Inc., licensed under MIT License)
- castle.windsor (Castle Project, licensed under Apache License Version 2.0.)
- xunit (Microsoft Public License)
- psake (by James Kovacs)
- costura (http://code.google.com/p/costura/, MIT Licence)