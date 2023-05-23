# CoupGame - by Aleix Cots Molina

Coup game implemented using Unity (version 2021.3.14f1)

------------------

Instructions:
- Open the game in Unity, open the Game Scene (set a resolution of 1200x800) and hit the play button.
- On your turn, select an action from the dropdown and hit the "Choose action" button. Also, you will be able to select an action to block or challenge another player.
- The last action will be displayed in the notifications text, but the full history of actions (and other relevant information) will be printed in Unity's console.
- When you finish the game, click on the restart button in the UI.

The game is ready to test, although there are many things to improve (including fixes to do). 

------------------

The UI is based in 4 parts:
- Notifications text in the top of the screen.
- Player agents panel below the notification text.
- Player (real) below the player agents panel.
- Deck panel in the left part of the screen.
- Restart button in the left bottom corner.
Any player panel has the name, the coins number and the cards of the player.
The real player has (in addition) a dropdown to choose an option and a button to use the chosen option.

For decisions, there is a base class ActionsController with an abstract method ChooseAction to allow anyone inherit and implement it. PlayerInputController implements it for the real player, and PlayerAIController does the same for the agents.

GameManager is the controller class, which acts as a bridge between the game logic and the UI, subscribing to events from CoupGame (send actions to players) and waiting for the players to choose the actions to send the choices back to CoupGame.

The Game logic is based in a main class CoupGame that initializes the CourtDeck with the Cards and coins, and the list of Players.
It also initializes the FiniteStateMachine that will direct the game using the different possible states which implement the logic of the game (PlayerTakeTurnState, OthersBlockState, AskForChallengeState, ReceiveChallengeState and SolveChallengeState).
When a state is entered, it decides what actions send to each player and tells it to CoupGame. When all the available players have chosen an action, the FSM will be updated and will decide what to do (block state, challenge state, next turn, etc.).
On the other side of game logic, there is the base class Action, which allows to write a name, a description, determine the card needed (and coins, if any), the blocking action if any, and a Perform method.
This Action class is not only useful for the main actions, but also to create blocking actions for the main actions and the Challenge action to send a challenge. Besides that, there is a Default action to allow the player to choose an "only-text" action like "Accept the block" or "Accept the challenge".

*(For more information about the code, take a look over the classes and their comments)*