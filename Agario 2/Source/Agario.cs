using Agario_2.Configs;
using Agario_2.Nodes;
using MyEngine;
using MyEngine.ConfigSystem;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.Nodes.UI;
using MyEngine.SoundSystem;
using MyEngine.Timed;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Agario_2;

public class Agario : Game
{
    private static FloatRect _mapBounds;
    private SceneNode _agarioScene;
    private SceneNode _pauseMenuScene;
    
    protected override void GameSpecificInitialization()
    {
        LoadConfigs();
        InitializeSounds();
        
        InitializeScenes();
        InitializePause();
    }

    private void InitializeScenes()
    {
        InitializeGameplayScene();
        InitializePauseMenu();
    }

    private void InitializeGameplayScene()
    {
        Scenes.Add("agario", SceneNode.CreateNewScene());
        _agarioScene = Scenes["agario"];

        AddUserPlayer();

        _agarioScene.AdoptChild(FoodPool.CreateFoodPool(MapConfigs.FoodAmount, _mapBounds));
        AddAiPlayers(MapConfigs.AiPlayersAmount);
    }

    private void InitializePauseMenu()
    {
        Scenes.Add("pause menu", SceneNode.CreateNewSceneWithCamera(Window));
        _pauseMenuScene = Scenes["pause menu"];
        _pauseMenuScene.IsRendered = false;
        
        _pauseMenuScene.AdoptChild(AgarioPauseMenu.CreateAgarioPauseMenu(Window.Size));

        _pauseMenuScene.GetDescendantOfType<Camera>().RenderedLayer = RenderLayer.UILayer;
    }

    private void InitializePause()
    {
        KeyBind bind = Input.GlobalListener.AddAction(new KeyBind("toggle pause", Keyboard.Key.Escape));
        bind.AddOnDownCallback(TogglePause);
    }

    public void TogglePause()
    {
        _pauseMenuScene.IsRendered = _agarioScene.IsProcessed;
        _agarioScene.IsProcessed = !_agarioScene.IsProcessed;
    }
    
    private void LoadConfigs()
    {
        FilePathsLibrary.LoadAndStorePathsFromFile(FilePathsLibrary.GetPath("texture files configs"));
        FilePathsLibrary.LoadAndStorePathsFromFile(FilePathsLibrary.GetPath("sound files configs"));
        
        ConfigLoader.LoadStaticFieldsFromFile(typeof(PlayerConfigs), FilePathsLibrary.GetPath("player configs"));
        ConfigLoader.LoadStaticFieldsFromFile(typeof(MapConfigs), FilePathsLibrary.GetPath("map configs"));
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
        SoundLibrary.LoadAndStoreSound(FilePathsLibrary.GetPath("dash"), "dash");
        SoundLibrary.StoreMusic(FilePathsLibrary.GetPath("invincible"), "invincible");
        
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