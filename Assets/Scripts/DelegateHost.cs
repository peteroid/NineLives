using UnityEngine;
using System.Collections;
using System;

public class DelegateHost
{

    private static DelegateHost sInstance = new DelegateHost();
    public static DelegateHost GetInstance()
    {
        return sInstance;
    }

    public static void Recreate()
    {
        sInstance = new DelegateHost();
    }

    public Action<bool, int> OnPressureChange;
    public static void PressureChange(bool state, int id)
    {
        if (sInstance.OnPressureChange != null)
        {
            sInstance.OnPressureChange.Invoke(state, id);
        }
    }

    public Action<int, int> OnCommandMove;
    public static void CommandMove(int dirX, int dirY)
    {
        if (sInstance.OnCommandMove != null)
        {
            sInstance.OnCommandMove.Invoke(dirX, dirY);
        }
    }

    public Action OnCommandMoveClear;
    public static void CommandMoveClear()
    {
        if (sInstance.OnCommandMoveClear != null)
        {
            sInstance.OnCommandMoveClear.Invoke();
        }
    }

    public Action OnCommandMoveRespond;
    public static void CommandMoveRespond()
    {
        if (sInstance.OnCommandMoveRespond != null)
        {
            sInstance.OnCommandMoveRespond.Invoke();
        }
    }
}
