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
        
        TextNode text = TextNode.CreateTextNode();
        text.MyText.DisplayedString = "Game is paused";
        text.MyText.Position = new(Window.Size.X / 2, Window.Size.Y / 2);
        _pauseMenuScene.AdoptChild(text);
        
        Text t = text.MyText;
        TimedSequence<string> sequence = 
            new(
                new() {
                    (0.0f, "Game is paused"),
                    (0.4f, "Game is paused."),
                    (0.8f, "Game is paused.."),
                    (1.2f, "Game is paused..."),
                    (1.6f, "Game is paused"), 
                    (2.0f, "Game is paused"), 
                }, 
                (string newDisplayedString) => t.DisplayedString = newDisplayedString
            );
        
        SequenceNode<string> sequenceNode = SequenceNode<string>.CreateSequenceNode(sequence);
        sequenceNode.Sequence.Play();
        sequenceNode.Sequence.OnFinished = sequenceNode.Sequence.Restart;
        text.AdoptChild(sequenceNode);

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