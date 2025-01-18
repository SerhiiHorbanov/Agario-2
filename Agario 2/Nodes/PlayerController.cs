using MyEngine;
using MyEngine.Nodes;

namespace Agario_2.Nodes;

public class PlayerController : Node
{
    private Player _player;

    private PlayerController(Player player)
        => _player = player;

    public static PlayerController CreatePlayerController(Player player)
        => new(player);
    
    protected override void ProcessInput()
        => _player.WishedDelta = Input.MousePositionFromWindowCenter;
}