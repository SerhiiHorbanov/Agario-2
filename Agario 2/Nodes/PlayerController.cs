using MyEngine;
using MyEngine.MyInput;
using MyEngine.Nodes;

namespace Agario_2.Nodes;

public class PlayerController : Node, IProcessesInput
{
    private Player _player;

    private PlayerController(Player player)
        => _player = player;

    public static PlayerController CreatePlayerController(Player player)
        => new(player);
    
    public void ProcessInput()
        => _player.WishedDelta = MouseInput.MousePositionFromWindowCenter;
}