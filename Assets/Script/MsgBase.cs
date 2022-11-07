using UnityEngine;
using System.Collections;

public class MsgBase
{
    //ushort 65535 ����Ϣ�������ַ���
    public ushort msgID;

    public MsgBase(ushort tmpMsg)
    {
        msgID = tmpMsg;
    }

    public ManagerID GetManager()
    {
        int tmpId = msgID / FrameTools.MsgSpan;
        return (ManagerID)(tmpId * FrameTools.MsgSpan);
    }
}
