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
        Input.GlobalListener.AddAction(new KeyBind("dash", Keyboard.Key.Space));
        Input.GlobalListener.AddAction(new KeyBind("body swap", Keyboard.Key.F));
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom in", false)).AddCallback(() => CurrentCamera.Size /= 1.2f);
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom out", true)).AddCallback(() => CurrentCamera.Size *= 1.2f);
    }

    private void AddUserPlayer()
    {
        Vector2f position = MapBounds.RandomPositionInside();
        Player player = Player.CreatePlayer(position);
        
        Input.GlobalListener.GetAction<KeyBind>("dash").AddOnDownCallback(player.Dash);
        Input.GlobalListener.GetAction<KeyBind>("body swap").AddOnDownCallback(player.SwapBodies);
        
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