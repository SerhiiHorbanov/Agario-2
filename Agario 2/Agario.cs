using MyEngine;
using SFML.System;

namespace Agario_2;

public class Agario : Game
{
    private const int MapSize = 5000;
    protected override void GameSpecificInitialization()
    {
        AddUserPlayer();

        AddFood(1000);
        AddAiPlayers(15);
    }

    private void AddUserPlayer()
    {
        Vector2f position = GetRandomPointInMap();
        Player player = Player.CreatePlayer(position);
        
        Root.AdoptChild(player);
        player.DraggedCamera = CurrentCamera;
    }

    private void AddAiPlayers(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAiPlayer();
    }

    private void AddAiPlayer()
    {
        Vector2f position = GetRandomPointInMap();
        Root.AdoptChild(Player.CreateAiPlayer(position));
    }

    private static Vector2f GetRandomPointInMap()
    {
        float x = Random.Shared.Next(MapSize);
        float y = Random.Shared.Next(MapSize);

        return new(x, y);
    }

    private void AddFood(int amount)
    {
        for (int i = 0; i < amount; i++) 
            AddFood();
    }

    private void AddFood()
    {
        Vector2f position = GetRandomPointInMap();
        Root.AdoptChild(EatableCircle.CreateEatableCircle(10, position));
    }
}