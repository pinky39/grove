# MAGICGROVE v 2.0

Magicgrove is a 2 player fantasy card game. You and computer are each given a
deck of cards which you use as best as you can to win the game. The most common
way to win is to reduce your opponent life to 0. There are currently 550 unique
cards available.

## Requirements & Installation

A modern computer with Windows operating system is required. Although the
game  can run on single core machines a multiple core machine is recommended.
.Net Framework 4.0 is required to play the game.

To install, unzip to a folder of your choice. Run grove.exe to play the
game.

## Game types

### Single match

You select a preconstructed deck for you and your opponent. Whoever wins 2 games
first wins the match.

### Single random match

You and your opponent are given 2 random decks with same ratings. Whoever wins 2
games first wins the match.

### Sealed tournament

Every player is given one starter pack and three booster packs. From that pool
of cards, and adding in as many basic land as desired, each player must build a
deck of at least 40 cards. Any opened cards not put in the main deck count as
part of the sideboard. In sealed deck, the skill is making the best out of what
you're given.

### Booster draft

Players are seated randomly at the table. Each player is given 3 booster  packs.
Every player opens their pack and choose one card from it, then  passes the rest
of the pack to the player on his left. Once everyone has  passed their packs,
pick up the next pack (located on your right), pick the  best card for your deck
from that pack and put it in your pile, and again  pass it to the neighbor on
your left. This process continues until all the  cards from the pack have been
picked. Then each player opens his or her  next pack, picks a card, and passes
the pack to the right (Packs go left,  right, left.). Once you have 45 cards in
your pile, it is time to build your deck.  Booster Draft rules allow you to add
as much basic land as you want to your  deck, and require that the deck be at
least 40 cards.

## Gameplay

### Saving & Loading

Every game can be saved via the in game menu (alt-q). Games can be loaded via
the Load saved game screen.

### Deck editor

To modify existing decks or add new ones you can use deck editor. Card library
can be filtered by mana color or by searching for specific text in the cards.
Card names are searched by default. To search other parts of the card specify
field name followed by a colon. The following field names are supported:

 - name
 - text
 - flavor
 - type
 - power
 - toughness

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

Transparent - priority is automatically passed on active & passive turns.
Green       - priority is automatically passed on passive turns.
Yellow      - priority is automatically passed on active turn.
Red         - priority is never automatically passed.

When an opponent plays a spell auto priority passing is disabled.

### Shortcuts

Spacebar    - moves to next step (passes priority)
Alt+q       - displays in game menu screen.
F11         - toggle between window and full screen mode

## Themes

Game visuals can be tweaked by modifying the contents of the media folder.

If you wish you can add your own card pictures to the /media/cards folder.
Picture size has to be 410x326 pixels, their names must match corresponding
card names. Basic lands have 4 versions eg. forest1.jpg, ... , forest4.jpg.

If you wish you can add your custom avatars to the media/avatars folder. Each
player is given a random avatar from this folder.

Additional battlefield backgrounds can be added to /media/images folder.
The filename must start with the word 'battlefield' e.g 'battlefield-1.jpg',
'battlefield-2.png'... At the start of the game a random battlefield background
is chosen amongst all available.

Other images can also be replaced, as long as they have the same size as the
originals.

## Adding new cards

Currently new cards can only be added by modifying the program source code.
Check project home page for tutorials: http://code.google.com/p/magicgrove/.

## Source code

The source code is available at http://code.google.com/p/magicgrove/

## License

The project source code is licensed under GPL-3 License.

## Credits

The project uses the following libraries with corresponding licenses:

- log4net library (Apache Software Foundation)
- caliburn.micro library (Blue Spire Consulting)
- castle.windsor (Castle Project)
- xunit
- psake (by James Kovacs)
- costura (http://code.google.com/p/costura/)

In game menu quote: 'Is it a mistake to think you can solve any major problems
just with potatoes?' is by Douglas Adams.

Some graphics are taken from https://openclipart.org/.
Some loading messages are taken from:
http://www.stackprinter.com/export?service=stackoverflow&question=182112
