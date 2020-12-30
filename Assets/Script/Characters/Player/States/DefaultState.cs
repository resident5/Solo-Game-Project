using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : IState
{
    private NewPlayer player;
    public void End()
    {
        throw new System.NotImplementedException();
    }

    public void Start(NewPlayer player)
    {
        this.player = player;
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
