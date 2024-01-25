using UnoConsole.Interface;
using UnoConsole.Enum;

namespace UnoConsole.Class;

public class PlayerData
{
  public IPlayer Player { get; } //duplicate IPlayer
  public List<ICard> CardsInHand { get; }
  public PlayerStatus PlayerStatus { get; set; }
  public bool HasDrawnCard { get; set; }
  // private int Points { get; set; }

  public PlayerData(IPlayer player)
  {
    Player = player;
    CardsInHand = [];
    PlayerStatus = PlayerStatus.Inactive;
    HasDrawnCard = false;
    // Points = 0;
  }

}
