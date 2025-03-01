using MyEngine;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.ResourceLibraries;

namespace SeaBattle;

public class SeaBattleGame : Game
{
    protected override void GameSpecificInitialization()
    {
        InitializeResources();
        InitializeGameplay();
    }

    private void InitializeResources()
    {
        TextureLibrary.LoadAndStoreTextureFromPathsLibrary("hidden cell");
        TextureLibrary.LoadAndStoreTextureFromPathsLibrary("shot ship cell");
        TextureLibrary.LoadAndStoreTextureFromPathsLibrary("ship cell");
        TextureLibrary.LoadAndStoreTextureFromPathsLibrary("empty cell");
        
        MapCell.InitializeLoadedCellTextures();
    }

    private void InitializeGameplay()
    {
        PlayerMap firstPlayerMap = PlayerMap.CreateMap(new(100, 100));
        PlayerMap secondPlayerMap = PlayerMap.CreateMap(new(800, 100));
        
        SeaBattleGameRules rules = SeaBattleGameRules.CreateRules(firstPlayerMap, secondPlayerMap, Input, Window.Close);
        Scenes.Add("sea battle", SceneNode.CreateNewScene());
        SceneNode scene = Scenes["sea battle"];
        
        Camera camera = Camera.CreateCamera(Window);
        camera.Size = new(1600, 900);
        camera.LeftTop = new(0, 0);
        
        scene.AdoptChild(camera);
        scene.AdoptChild(firstPlayerMap);
        scene.AdoptChild(secondPlayerMap);
    }
}