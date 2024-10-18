using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameServer
{
    void Init();
    void GetCurrentGameState(Action<BattleState> callback);
}