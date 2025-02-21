using System.Xml;
using MyEngine.MyInput;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.Nodes.UI;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class SkinSelect : Node
{
    private Action<Color> _startGameAction;

    private SkinPreview _skinPreview;
    private int _currentSkinIndex;
    
    private static readonly Vector2f SkinPreviewAnchor = new(0.5f, 0.5f);
    
    private static readonly Vector2f SelectButtonAnchor = new(0.5f, 1);
    private static readonly Vector2i SelectButtonOffset = new(-64, -74);

    private static readonly Vector2i NextSkinButtonOffset = new(-128, 0);
    private static readonly Vector2f NextSkinButtonAnchor = new(1, 0.5f);

    private static readonly Vector2i PreviousSkinButtonOffset = new(64, 0);
    private static readonly Vector2f PreviousSkinButtonAnchor = new(0, 0.5f);
    
    private int CurrentSkinIndex
    {
        get => _currentSkinIndex;
        set
        {
            if (value < 0)
                value = Colors.Length + (value % Colors.Length);
            
            if (value >= Colors.Length)
                value %= Colors.Length;
            
            _skinPreview.Color = Colors[value];
            _currentSkinIndex = value;
        }
    }
    
    private Color[] Colors
        => EatableCircle.Colors;
    
    private SkinSelect()
    { }
    
    public static SkinSelect Create(RenderTarget target, InputSystem input, Action<Color> startGame)
    {
        SkinSelect result = new();

        Camera camera = result.AdoptChild(Camera.CreateUICamera(target));

        result.InitializeSkinPreview(camera);
        result.InitializeSelectButton(input, startGame, result);
        result.InitializeDirectionButtons(camera, input);
        
        return result;
    }

    private void InitializeSkinPreview(Camera camera)
    {
        _skinPreview = AdoptChild(SkinPreview.CreatePreview(camera, Colors[_currentSkinIndex]));
        
        _skinPreview.AnchorOnTarget = SkinPreviewAnchor;
        _skinPreview.RenderLayer = RenderLayer.UILayer;
    }

    private void InitializeSelectButton(InputSystem input, Action<Color> startGame, SkinSelect result)
    {
        Button selectButton = result.AdoptChild(Button.CreateButton(result.GetChildOfType<Camera>(), input, "select skin button"));
        
        selectButton.AnchorOnTarget = SelectButtonAnchor;
        selectButton.Offset = SelectButtonOffset;
        
        result._startGameAction = startGame;
        selectButton.OnPressed += result.StartGame;
    }

    private void InitializeDirectionButtons(Camera camera, InputSystem input)
    {
        Button previousSkinButton = AdoptChild(Button.CreateButton(camera, input, "previous skin button"));
        Button nextSkinButton = AdoptChild(Button.CreateButton(camera, input, "next skin button"));
        
        previousSkinButton.AnchorOnTarget = PreviousSkinButtonAnchor;
        nextSkinButton.AnchorOnTarget = NextSkinButtonAnchor;
        
        previousSkinButton.Offset = PreviousSkinButtonOffset;
        nextSkinButton.Offset = NextSkinButtonOffset;

        previousSkinButton.OnPressed += Previous;
        nextSkinButton.OnPressed += Next;
    }

    private void StartGame()
        => _startGameAction(Color.Cyan);

    private void Next()
    {
        CurrentSkinIndex++;
    }

    private void Previous()
    {
        CurrentSkinIndex--;
    }
}