# MAGICGROVE

Magicgrove is a computer implementation of a trading card game: Magic: The
Gathering. The goal of the project is to implement ai algorithms which can play
the game at the same level as a casual human player. Currently there are 690
unique cards implemented, mostly from Urza's block (593 out of 616 cards are
available).

## Requirements & Installation

A modern computer with Windows operating system is required. Although the
game  can run on single core machines a multiple core machine is recommended.
.Net Framework 4.0 is required to play the game.

To install, unzip to a folder of your choice. Run grove.exe to play the
game.

## Gameplay and features

You can play either

 * a single match,
 * a sealed tournament or
 * a draft tournament.

You can save the game at any time by pressing alt-q and choosing Save game
menu option.

### Deck editor

To modify existing decks or add new ones you can use deck editor. Card library
can be filtered by mana color or by searching for specific text in the cards.
Card names are searched by default. To search other parts of the card specify
field name followed by a colon. The following field names are supported:

 * name
 * text
 * flavor
 * type
 * power
 * toughness

Conditions can be joined by using AND or OR (note the uppercase).
For example:

text:flying AND power:5

would list only cards that have flying in the text and have power of 5.

Queries use lucene query syntax which is available here:
http://www.lucenetutorial.com /lucene-query-syntax.html.

### Tapping lands for mana

Tapping lands is not needed in most of the cases, just click the spell and the
mana will be payed automatically. If you want to have absolute control, you can
tap the lands manually though.

### Priority passing

Priority is by default automatically passed for each step, except for
First main (active turn), Second main (active turn) and
Declare blockers (active & passive turns).

You can change defaults by clicking on corresponding step's button.
A button has 4 states:

 * Transparent - priority is automatically passed on active & passive turns.
 * Green       - priority is automatically passed on passive turns.
 * Yellow      - priority is automatically passed on active turn.
 * Red         - priority is never automatically passed.

When an opponent plays a spell auto priority passing is disabled.

### Shortcuts

 * Spacebar    - moves to next step (passes priority)
 * Enter       - confirms attackers or blockers selection
 * Alt+q       - displays in game menu screen.
 * F11         - toggle between window and full screen mode

## Themes

Game visuals can be tweaked by modifying the contents of the media folder.

If you wish you can add your own card pictures to the /media/cards folder or to
the cards.zip file. Picture size has to be 410x326 pixels, their names must
match corresponding card names. Basic lands have 15 versions eg. forest1.jpg,
... , forest15.jpg.

If you wish you can add your custom avatars to the media/avatars folder.

Additional battlefield backgrounds can be added to /media/images folder.
The filename must start with the word 'battlefield' e.g 'battlefield-1.jpg',
'battlefield-2.png'... At the start of the game a random battlefield background
is chosen amongst all available.

Other images can also be replaced, as long as they have the same size as the
originals.

## How AI is implemented

Magic: The Gathering is a game with hidden state. On every move, computer
simulates future moves and builds the game tree. Hidden information is not
available during simulation (computer is not cheating). Every leaf node is
scored and the best move is chosen according to min/max rule. Because the
unpruned game tree would take too long to evaluate, heuristics are used to
choose if the branch is expanded or not. Ai uses special rules for:

 * target selection,
 * timing of spells and abilities,
 * X cost calculation,
 * combat simulation,
 * mana payments,
 * repeated activations of abilities,
 * special card decisions

The game currently uses 60 steps look ahead, the search time rarely exceeds 5
seconds (on quad core machine).

### Deck generation
Limited decks are automatically generated from given starter and booster packs.
Computer first creates multiple decks for various color combinations, and then
chooses the one which wins a simulated tournament.

### Draft
Drafting AI is based on drafting strategies described here:
http://archive.wizards.com/Magic/magazine/article.aspx?x=mtgcom/academy/39.

## License

The project source code is licensed under GPL-3 License.

## Credits

Magic: The Gathering was created by Richard Garfield and is published and
developed by Wizards of the Coast.

This project uses the following open source libraries:

* log4net library (Apache Software Foundation)
* caliburn.micro library (Blue Spire Consulting)
* castle.windsor (Castle Project)
* xunit
* psake (by James Kovacs)
* costura (http://code.google.com/p/costura/)
* lucene.net (http://lucenenet.apache.org/)

In game menu quote: 'Is it a mistake to think you can solve any major problems
just with potatoes?' is by Douglas Adams.

Some graphics are taken from https://openclipart.org/.
Some loading messages are taken from:
http://www.stackprinter.com/export?service=stackoverflow&question=182112