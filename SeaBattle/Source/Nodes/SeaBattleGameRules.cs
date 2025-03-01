using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes;
using SFML.System;
using SFML.Window;

namespace SeaBattle.Nodes;

public class SeaBattleGameRules : Node
{
    private PlayerMap _firstPlayerMap;
    private PlayerMap _secondPlayerMap;
    private PlayerMap _activePlayerMap;
    private PlayerMap _waitingPlayerMap;
    
    public static Action _endGame;
    
    public UpdateLayer UpdateLayer
        => UpdateLayer.Normal;
    
    private SeaBattleGameRules()
    { }

    public static SeaBattleGameRules CreateRules(PlayerMap first, PlayerMap second, InputSystem input, Action closeWindow)
    {
        SeaBattleGameRules rules = new();
        
        rules._firstPlayerMap = first;
        rules._secondPlayerMap = second;
        rules._activePlayerMap = rules._firstPlayerMap;
        rules._waitingPlayerMap = rules._secondPlayerMap;
        rules._waitingPlayerMap.IsHidden = true;

        _endGame = closeWindow;
        
        rules.InitializeInput(input);

        return rules;
    }


    private void InitializeInput(InputSystem input)
    {
        InputListener global = input.GlobalListener;
        
        EnsureKeyBindSet(global, "up", Keyboard.Key.W, GoUp);
        EnsureKeyBindSet(global, "down", Keyboard.Key.S, GoDown);
        EnsureKeyBindSet(global, "left", Keyboard.Key.A, GoLeft);
        EnsureKeyBindSet(global, "right", Keyboard.Key.D, GoRight);
        EnsureKeyBindSet(global, "shoot", Keyboard.Key.Space, Shoot);
    }

    private void Shoot()
    {
        ShootingResult shootingResult = _waitingPlayerMap.ShootAtCursor();
        
        if (shootingResult == ShootingResult.Hit)
        {
            if (!_waitingPlayerMap.HasNotShotShip())
                _endGame();
            return;
        }
        
        SwapPlayersForNextTurn();
    }

    private void SwapPlayersForNextTurn()
    {
        (_activePlayerMap, _waitingPlayerMap) = (_waitingPlayerMap, _activePlayerMap);
        _activePlayerMap.IsHidden = false;
        _waitingPlayerMap.IsHidden = true;
    }

    private void EnsureKeyBindSet(InputListener global, string name, Keyboard.Key key, Action callback)
    {
        KeyBind keyBind = global.GetAction<KeyBind>(name);
        
        if (keyBind == null)
            keyBind = global.AddAction(new KeyBind(name, key));
        
        keyBind.ResetOnStartedCallbacks(callback);
    }

    private void MoveCursor(Vector2i delta)
    {
        _activePlayerMap.MoveCursor(delta);
        _waitingPlayerMap.MoveCursor(delta);
    }
    
    private void GoUp()
        => MoveCursor(new(0, -1));
    private void GoDown()
        => MoveCursor(new(0, 1));
    private void GoLeft()
        => MoveCursor(new(-1, 0));
    private void GoRight()
        => MoveCursor(new(1, 0));
}