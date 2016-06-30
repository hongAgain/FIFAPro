
using System;

public interface IState
{
    void OnEnter();
    void OnExit();
    void OnUpdate(float fTime);
}
