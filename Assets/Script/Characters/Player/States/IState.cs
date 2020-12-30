using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Start(NewPlayer player);

    void Update();          

    void End();
}
