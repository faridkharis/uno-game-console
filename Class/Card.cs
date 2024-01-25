using UnoConsole.Interface;
using UnoConsole.Enum;

namespace UnoConsole.Class;

public class Card : ICard
{
	public int CardId { get; private set; }
	public CardColor CardColor { get; private set; }
	public CardValue CardValue { get; private set; }
	public CardEffect CardEffect { get; private set; }

	public Card(int cardId, CardColor cardCcolor, CardValue cardValue, CardEffect cardEffect)
	{
		CardId = cardId;
		CardColor = cardCcolor;
		CardValue = cardValue;
		CardEffect = cardEffect;
	}

	public static CardColor GetCardColor()
	{
		return CardColor.Blue;
	}
	public static CardValue GetCardType()
	{
		return CardValue.Skip;
	}
	public static int GetCardValue(CardValue cardValue)
	{
		int valueFromCard = (int)cardValue;
		return valueFromCard;
	}

	public override string ToString()
	{
		return $"{CardColor} {CardValue}";
	}

}
