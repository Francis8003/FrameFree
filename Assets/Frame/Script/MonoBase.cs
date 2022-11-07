using UnityEngine;
using System.Collections;

public abstract class MonoBase : MonoBehaviour
{
    public abstract void ProcessEvent(MsgBase msg);
}
