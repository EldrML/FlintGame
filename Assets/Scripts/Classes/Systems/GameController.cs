using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoSingleton<GameController>
{

    protected override GameController GetSingletonInstance { get { return this; } }

    public event Action GameStarted;

    public void StartNewGame()
    {
        GameStarted?.Invoke();
    }

    public void LoadGame()
    {
        throw new NotImplementedException();
    }

    public void SaveGame()
    {
        throw new NotImplementedException();
    }
}
