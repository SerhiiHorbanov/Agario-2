using Agario_2.Configs;
using Agario_2.Nodes;
using MyEngine;
using MyEngine.ConfigSystem;
using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2;

public class Agario : Game
{
    private static FloatRect MapBounds;
    
    protected override void GameSpecificInitialization()
    {
        LoadConfigs();
        
        InitializeKeyBinds();

        AddUserPlayer();

        Root.AdoptChild(FoodPool.CreateFoodPool(MapConfigs.FoodAmount, MapBounds));
        AddAiPlayers(MapConfigs.AiPlayersAmount);
    }

    private void LoadConfigs()
    {
        ConfigLoader.LoadStaticFieldsFromFile(typeof(PlayerConfigs), "Player.cfg");
        ConfigLoader.LoadStaticFieldsFromFile(typeof(MapConfigs), "Map.cfg");
        MapBounds = new(0, 0, MapConfigs.SizeHorizontal, MapConfigs.SizeVertical);
    }

    private void InitializeKeyBinds()
    {
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom in", false)).AddCallback(() => CurrentCamera.Size /= 1.2f);
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom out", true)).AddCallback(() => CurrentCamera.Size *= 1.2f);
    }

    private void AddUserPlayer()
    {
        Vector2f position = MapBounds.RandomPositionInside();
        
        Player player = Player.CreatePlayerWithNoController(position);
        player.DraggedCamera = CurrentCamera;
        
        Root.AdoptChild(player);
        Root.AdoptChild(PlayerController.CreatePlayerController(Input, player));
    }

    private void AddAiPlayers(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAiPlayer();
    }

    private void AddAiPlayer()
    {
        Vector2f position = MapBounds.RandomPositionInside();
        Player player = Player.CreatePlayerWithNoController(position);
        
        Root.AdoptChild(player);
        Root.AdoptChild(AiController.CreateAiController(player));
    }
}