using UnityEngine;
using System.Collections;
using System;

public class DelegateHost
{
    public static Action<bool, int> OnPressureChange;
    public static Action<int, int> OnCommandMove;
}
