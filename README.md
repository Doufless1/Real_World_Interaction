# Real_World_Interaction

Our project consists of 2 projects BlackServer and Batman(this is the blackjack game).

#BlackServer
is a host server specially created to create and host the deck for the game. Once the code is run it will create a local server and an instance of the deck.
It is meant for one player and only hosts one instance of deck however multiple players can asynchronously connect and disconnect as they want with the server. Once it accepts a connection the server will display the message received and connection status. Only 3 commands are implemented. These are "HIT" which returns on card objects, "DEAL" which returns  2 card objects and "NEW" which creates a new shuffled deck of cards.

#Batman
hosts the game logic and GUI interface. The player can actually interact with the game through the GUI, but since we get our deck from the server, we recommend that you start running that first. Once the player presses "New Game" players are required to take a bet to proceed. The game will connect to the server, retrieve the cards needed, and disconnect. Players Can hit  for an additional card, fold to quit the current hand, double to take one more card and double their bet.  If the player wins the hand money is added to the player. The game will repeat until the player runs out of money.

