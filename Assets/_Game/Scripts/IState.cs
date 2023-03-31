using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(Bot bot);

    public void Execute(Bot bot);

    public void Exit(Bot bot);
}
