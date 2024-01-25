using Spectre.Console;
using UnoConsole.Class;
using UnoConsole.Interface;

class Program
{
	static void Main(string[] args)
	{
		var numberOfPlayers = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
				.Title("Select Number of Player")
				.AddChoices([
						"2", "3", "4"
				]));

		GameController gc = new();

		gc.StartGame(int.Parse(numberOfPlayers));

		while (!gc.IsGameFinished())
		{
			var currentPlayer = gc.GetCurrentPlayer();
			Console.WriteLine($"Current Turn: {currentPlayer.Player.Name}");
			Console.WriteLine($"Discard Pile : {gc.discardPile.Last().CardColor} - {gc.discardPile.Last().CardValue}\n");

			ICard? playableCard = gc.CheckPlayableCards(currentPlayer.CardsInHand);
			if (playableCard != null)
			{
				var cardOption = currentPlayer.CardsInHand;

				var selectedCard = AnsiConsole.Prompt(
					new SelectionPrompt<Card>()
						.Title("Select Card")
						.AddChoices(
							cardOption.ConvertAll(c => (Card)c)
						));

				if (gc.CanPlayCard(selectedCard))
				{
					gc.PlayCard(currentPlayer.Player, selectedCard);
					gc.EndTurn();
					Console.Clear();

				}
				else
				{
					Console.WriteLine("Card cannot be played, choose another card");
				}
			}
			else
			{
				if (!currentPlayer.HasDrawnCard)
				{
					Console.WriteLine("No cards can be played");
					ICard drawnCard = gc.DrawCardFromStockPile();
					currentPlayer.CardsInHand.Add(drawnCard);
					Console.WriteLine($"{currentPlayer.Player.Name} drew a card: {drawnCard.CardColor} - {drawnCard.CardValue}");
					currentPlayer.HasDrawnCard = true;
					Console.ReadKey();
					Console.Clear();
				}
				else
				{
					gc.EndTurn();
					Console.Clear();
					currentPlayer.HasDrawnCard = false;
				}
			}
		}
		Console.WriteLine($"Winner : {gc.GetWinner().Name}");

	}
}