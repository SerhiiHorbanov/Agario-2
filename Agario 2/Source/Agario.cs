using Agario_2.Configs;
using Agario_2.Nodes;
using MyEngine;
using MyEngine.ConfigSystem;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.SoundSystem;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2;

public class Agario : Game
{
    private static FloatRect _mapBounds;
    private SceneNode _agarioScene;
    
    protected override void GameSpecificInitialization()
    {
        LoadConfigs();
        InitializeSounds();
        
        InitializeScenes();
    }

    private void InitializeScenes()
    {
        Scenes.Add("aga", SceneNode.CreateNewScene());
        _agarioScene = Scenes["aga"];
        
        AddUserPlayer();

        _agarioScene.AdoptChild(FoodPool.CreateFoodPool(MapConfigs.FoodAmount, _mapBounds));
        AddAiPlayers(MapConfigs.AiPlayersAmount);
    }

    private void LoadConfigs()
    {
        ConfigLoader.LoadStaticFieldsFromFile(typeof(PlayerConfigs), "Configs/Player.cfg");
        ConfigLoader.LoadStaticFieldsFromFile(typeof(MapConfigs), "Configs/Map.cfg");
        _mapBounds = new(new(), MapConfigs.Size);
    }

    Camera AddConfiguredCamera()
    {
        Camera camera = Camera.CreateCamera(Window);

        _agarioScene.AdoptChild(camera);
        InitializeKeyBinds(camera);

        return camera;
    }
    
    private void InitializeKeyBinds(Camera camera)
    {
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom in", false)).AddCallback(() => camera.Size /= 1.2f);
        Input.GlobalListener.AddAction(new WheelScrollBind("zoom out", true)).AddCallback(() => camera.Size *= 1.2f);
    }

    private void InitializeSounds()
    {
        SoundFilesConfigs configs = ConfigLoader.LoadFromFile<SoundFilesConfigs>("Configs/SoundFiles.cfg");
        
        SoundLibrary.LoadAndStoreSound(configs.DashFile, "dash");
        SoundLibrary.StoreMusic(configs.InvincibleFile, "invincible");
        
        SoundManager.CreateMusic("invincible").WithOptions(new SoundOptions(loop: true, volume: 10)).Play();
    }

    private void AddUserPlayer()
    {
        Vector2f position = _mapBounds.RandomPositionInside();
        
        Player player = Player.CreatePlayerWithNoController(position);
        player.DraggedCamera = AddConfiguredCamera();
        
        _agarioScene.AdoptChild(player);
        _agarioScene.AdoptChild(PlayerController.CreatePlayerController(Input, player));
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
        
        _agarioScene.AdoptChild(player);
        _agarioScene.AdoptChild(AiController.CreateAiController(player));
    }
}