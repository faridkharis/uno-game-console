using UnoConsole.Interface;
using UnoConsole.Enum;

namespace UnoConsole.Class;

public class GameController
{
	// Properties
	private List<ICard> Cards { get; }
	private List<IPlayer> Players { get; }
	public Dictionary<IPlayer, PlayerData> PlayersData { get; }
	private Direction Direction { get; set; }

	// Variables
	private readonly int maxCardsInHand = 7;
	public List<ICard> stockPile = [];
	public List<ICard> discardPile = [];
	public Direction currentDirecton = Direction.Clockwise;
	private int currentPlayerIndex;

	// Constructor
	public GameController()
	{
		PlayersData = new Dictionary<IPlayer, PlayerData>();
		Cards = new List<ICard>();
		Players = [];
		currentPlayerIndex = 0;
	}

	// Methods
	public void StartGame(int numberOfPlayers)
	{
		AddPlayers(numberOfPlayers);
		GenerateCards();
		ShuffleCards();
		DistributeCards();
		AddStockPile();
		DrawInitialCard();
		SetPlayerTurn(PlayersData.Keys.First());
	}
	public void AddPlayers(int numberOfPlayers) // add params IPlayer
	{
		for (int i = 1; i <= numberOfPlayers; i++)
		{
			IPlayer player = new Player(i, $"Player {i}");
			PlayerData playerData = new(player);
			PlayersData.Add(player, playerData);
		}

	}
	public List<ICard> GenerateCards() // return type IEnum
	{
		int cardId = 1;

		foreach (CardColor color in CardColor.GetValues(typeof(CardColor)))
		{
			if (color != CardColor.Black)
			{
				for (CardValue value = CardValue.Zero; value <= CardValue.DrawTwo; value++)
				{
					CardEffect effect = CardEffect.None;
					Cards.Add(new Card(cardId++, color, value, effect));
					if (value != CardValue.Zero)
					{
						Cards.Add(new Card(cardId++, color, value, effect));
					}
				}
			}
		}

		// Adding Wild and Wild Draw Four cards
		// for (int i = 0; i < 4; i++)
		// {
		//   Cards.Add(new Card(cardId++, CardColor.Black, CardValue.Wild, CardEffect.Wild));
		//   Cards.Add(new Card(cardId++, CardColor.Black, CardValue.WildDrawFour, CardEffect.WildDrawFour));
		// }

		return Cards;
	}
	public void ShuffleCards()
	{
		Random random = new();
		int n = Cards.Count;

		while (n > 1)
		{
			n--;
			int k = random.Next(n + 1);
			(Cards[n], Cards[k]) = (Cards[k], Cards[n]);
		}
	}
	public void DistributeCards()
	{
		foreach (var playerData in PlayersData)
		{
			for (int i = 0; i < maxCardsInHand; i++)
			{
				// add action delegate
				ICard card = Cards.First();
				Cards.RemoveAt(0);
				playerData.Value.CardsInHand.Add(card);
			}
		}
	}
	public void AddStockPile()
	{
		stockPile = Cards;
	}
	public void DrawInitialCard()
	{
		if (stockPile.Count <= 0)
		{
			ReshuffleDiscardPile();
		}
		else
		{
			discardPile.Add(Cards[0]);
			stockPile.RemoveAt(0);
		}
	}
	public PlayerData GetCurrentPlayer()
	{
		return PlayersData.Values.ElementAt(currentPlayerIndex);
	}
	public void SetPlayerTurn(IPlayer player)
	{
		PlayersData[player].PlayerStatus = PlayerStatus.Active;
	}
	public void EndTurn() //bool
	{
		var currentPlayer = PlayersData.Keys.ElementAt(currentPlayerIndex);
		PlayersData[currentPlayer].PlayerStatus = PlayerStatus.Inactive;

		currentPlayerIndex = (currentPlayerIndex + 1) % PlayersData.Count;
		var nextPlayer = PlayersData.Keys.ElementAt(currentPlayerIndex);

		SetPlayerTurn(nextPlayer);
	}


	public void PlayCard(IPlayer player, ICard card) //bool
	{
		if (PlayersData.TryGetValue(player, out PlayerData? value) && value.PlayerStatus == PlayerStatus.Active)
		{

			HandleSpecialCard(player, card);

			discardPile.Add(card);
			value.CardsInHand.Remove(card);

			CheckUno(player);
		}
	}

	public bool CanPlayCard(ICard card)
	{
		ICard topDiscard = discardPile.Last();

		return card.CardColor == topDiscard.CardColor || card.CardValue == topDiscard.CardValue;
	}
	public ICard? CheckPlayableCards(List<ICard> hand)
	{
		return hand.FirstOrDefault(CanPlayCard);
	}
	public ICard DrawCardFromStockPile()
	{
		if (stockPile.Count == 0)
		{
			ReshuffleDiscardPile();
		}

		ICard drawnCard = stockPile[0];
		stockPile.RemoveAt(0);

		return drawnCard;
	}
	private void ReshuffleDiscardPile()
	{
		stockPile.AddRange(discardPile);
		discardPile.Clear();

		ShuffleCards();
	}

	private void HandleSpecialCard(IPlayer player, ICard card)
	{
		switch (card.CardEffect)
		{
			case CardEffect.None:
				break;

			case CardEffect.Skip:
				SkipPlayer(player);
				break;

			case CardEffect.Reverse:
				ReverseDirection();
				break;

			case CardEffect.Drawtwo:
				DrawTwoCards();
				break;

			case CardEffect.Wild:
				Wild();
				break;

			case CardEffect.WildDrawFour:
				WildDrawFour();
				break;
		}
	}

	private void SkipPlayer(IPlayer player)
	{
		Console.WriteLine($"Player {player.Name} played a Skip card. Next player's turn is skipped.");
		EndTurn();
	}

	private void ReverseDirection()
	{
		Console.WriteLine("Reverse card played. Turn order is reversed.");
	}

	private void DrawTwoCards()
	{
		Console.WriteLine("Draw Two card played. Next player draws two cards.");
	}

	private void Wild()
	{

	}

	private void WildDrawFour()
	{

	}

	private void CheckUno(IPlayer player)
	{

	}

	public bool IsGameFinished()
	{
		return PlayersData.Any(playerData => playerData.Value.CardsInHand.Count == 0);
	}
	public IPlayer GetWinner()
	{
		return PlayersData.First(playerData => playerData.Value.CardsInHand.Count == 0).Key;
	}

}
