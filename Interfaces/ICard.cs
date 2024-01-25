using UnoConsole.Enum;

namespace UnoConsole.Interface;

public interface ICard
{
  int CardId { get; }
  CardColor CardColor { get; }
  CardValue CardValue { get; }
  CardEffect CardEffect { get; }
}
