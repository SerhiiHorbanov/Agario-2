using Agario_2.Nodes;
using MyEngine;
using MyEngine.MyInput;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Agario_2;

public class Agario : Game
{
    private const int MapSize = 5000;
    private static readonly FloatRect MapBounds = new FloatRect(0, 0, 5000, 5000);
        
    private const int AiPlayersAmount = 20;
    private const int FoodAmount = 1000;
    
    protected override void GameSpecificInitialization()
    {
        InitializeKeyBinds();

        AddUserPlayer();

        Root.AdoptChild(FoodPool.CreateFoodPool(FoodAmount, MapBounds));
        AddAiPlayers(AiPlayersAmount);
    }

    private void InitializeKeyBinds()
    {
        Inputs.AddKeyBind("dash", Keyboard.Key.Space);
        Inputs.AddKeyBind("body swap", Keyboard.Key.F);
    }

    private void AddUserPlayer()
    {
        Vector2f position = MapBounds.RandomPositionInside();
        Player player = Player.CreatePlayer(position);
        
        Inputs.GetAction<KeyBind>("dash").AddOnDownCallback(player.Dash);
        Inputs.GetAction<KeyBind>("body swap").AddOnDownCallback(player.SwapBodies);
        
        player.DraggedCamera = CurrentCamera;
        
        Root.AdoptChild(player);
    }

    private void AddAiPlayers(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAiPlayer();
    }

    private void AddAiPlayer()
    {
        Vector2f position = MapBounds.RandomPositionInside();
        Root.AdoptChild(Player.CreateAiPlayer(position));
    }
}