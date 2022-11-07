using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventNode
{
    public MonoBase data;
    public EventNode next;

    public EventNode(MonoBase tmpMono)
    {
        this.data = tmpMono;
        this.next = null;
    }

}

public class ManagerBase : MonoBase
{
    //�洢ע����Ϣ����
    public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono">Ҫע��Ľű�</param>
    /// <param name="msgs">һ���ű�����ע����msg</param>
    public void RegistMsg(MonoBase mono, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode tmp = new EventNode(mono);
            RegistMsg(msgs[i], tmp);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">msgID</param>
    /// <param name="mono">ע���ʱ���б�</param>
    public void RegistMsg(ushort id, EventNode node)
    {
        //���������·��û�������ϢID
        if (!eventTree.ContainsKey(id))
        {
            eventTree.Add(id, node);
        }
        //����������ϢID��������Ϣ��ĩβ�������
        else
        {
            EventNode tmp = eventTree[id];
            while (tmp.next != null)
            {
                tmp = tmp.next;
            }
            tmp.next = node;
        }
    }


    public void unRegistMsg(ushort id, MonoBase node)
    {
        if (!eventTree.ContainsKey(id))
        {
            Debug.LogWarning("not contain id==" + id);
            return;
        }
        else
        {
            EventNode tmp = eventTree[id];
            if (node == tmp.data)
            {
                EventNode header = tmp;
                if (tmp.next == null)
                {

                }
                else
                {

                }
            }
        }

    }
}
