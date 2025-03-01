using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using SFML.System;
using SFML.Window;

namespace SeaBattle;

public class SeaBattleGameRules : Node
{
    private PlayerMap _firstPlayer;
    private PlayerMap _secondPlayer;
    private PlayerMap _activePlayer;
    private PlayerMap _waitingPlayer;
    
    public static Action _endGame;
    
    public UpdateLayer UpdateLayer
        => UpdateLayer.Normal;
    
    private SeaBattleGameRules()
    { }

    public static SeaBattleGameRules CreateRules(PlayerMap first, PlayerMap second, InputSystem input, Action closeWindow)
    {
        SeaBattleGameRules rules = new();
        
        rules._firstPlayer = first;
        rules._secondPlayer = second;
        rules._activePlayer = rules._firstPlayer;
        rules._waitingPlayer = rules._secondPlayer;
        rules._waitingPlayer.IsHidden = true;

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
        ShootingResult shootingResult = _waitingPlayer.ShootAtCursor();
        
        if (shootingResult == ShootingResult.Hit)
        {
            if (!_waitingPlayer.HasNotShotShip())
                _endGame();
            return;
        }
        
        SwapPlayersForNextTurn();
    }

    private void SwapPlayersForNextTurn()
    {
        (_activePlayer, _waitingPlayer) = (_waitingPlayer, _activePlayer);
        _activePlayer.IsHidden = false;
        _waitingPlayer.IsHidden = true;
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
        _firstPlayer.MoveCursor(delta);
        _secondPlayer.MoveCursor(delta);
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