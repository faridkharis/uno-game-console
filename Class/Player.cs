using UnoConsole.Interface;

namespace UnoConsole.Class;

public class Player : IPlayer
{
	public int Id { get; }
	public string Name { get; set; }

	public Player(int id, string name)
	{
		Id = id;
		Name = name;
	}
}
