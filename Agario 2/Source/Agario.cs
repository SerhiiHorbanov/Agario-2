using Agario_2.Configs;
using Agario_2.Nodes;
using MyEngine;
using MyEngine.ConfigSystem;
using MyEngine.MyInput.InputActions;
using MyEngine.SoundSystem;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2;

public class Agario : Game
{
    private static FloatRect _mapBounds;
    
    protected override void GameSpecificInitialization()
    {
        LoadConfigs();
        InitializeSoundLibrary();
        
        InitializeKeyBinds();
        AddUserPlayer();

        Root.AdoptChild(FoodPool.CreateFoodPool(MapConfigs.FoodAmount, _mapBounds));
        AddAiPlayers(MapConfigs.AiPlayersAmount);
    }

    private void LoadConfigs()
    {
        ConfigLoader.LoadStaticFieldsFromFile(typeof(PlayerConfigs), "Configs/Player.cfg");
        ConfigLoader.LoadStaticFieldsFromFile(typeof(MapConfigs), "Configs/Map.cfg");
        _mapBounds = new(new(), MapConfigs.Size);
    }

    private void InitializeKeyBinds()
    {
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom in", false)).AddCallback(() => CurrentCamera.Size /= 1.2f);
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom out", true)).AddCallback(() => CurrentCamera.Size *= 1.2f);
    }

    private void InitializeSoundLibrary()
    {
        SoundFilesConfigs configs = ConfigLoader.LoadFromFile<SoundFilesConfigs>("Configs/SoundFiles.cfg");
        
        SoundLibrary.LoadAndStoreSound(configs.DashFile, "dash");
    }
    
    private void AddUserPlayer()
    {
        Vector2f position = _mapBounds.RandomPositionInside();
        
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
        Vector2f position = _mapBounds.RandomPositionInside();
        Player player = Player.CreatePlayerWithNoController(position);
        
        Root.AdoptChild(player);
        Root.AdoptChild(AiController.CreateAiController(player));
    }
}