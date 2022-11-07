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
    /// <summary>
    /// ע�������Ϣ
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="msgs"></param>
    public void UnRegistMsg(MonoBase mono, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            UnRegistMsg(msgs[i], mono);
        }
    }

    /// <summary>
    /// ע��һ����Ϣ�ڵ�
    /// </summary>
    /// <param name="id"></param>
    /// <param name="node"></param>
    public void UnRegistMsg(ushort id, MonoBase node)
    {
        if (!eventTree.ContainsKey(id))
        {
            Debug.LogWarning("not contain id==" + id);
            return;
        }
        else//���������Ϣ
        {
            EventNode tmp = eventTree[id];
            //Ҫע���Ľڵ��ǵ�һ���ڵ�
            if (node == tmp.data)
            {
                EventNode header = tmp;

                //���˵�һ���ڵ��߻�����Ϣ
                if (header.next != null)
                {
                    header.data = tmp.next.data;
                    header.next = tmp.next.next;
                }
                //ֻ�е�һ���ڵ�
                else
                {
                    eventTree.Remove(id);
                }
            }
            else//Ҫע���Ľڵ����м����β������Ϣ 
            {
                while (tmp.next != null && tmp.next.data != null)//TBD
                {
                    tmp = tmp.next;
                }
                //ȥ���м�Ľڵ�
                if (tmp.next.next != null)
                {
                    tmp.next = tmp.next.next;
                }
                //ȥ��β���Ľڵ�
                else
                {
                    tmp.next = null;
                }
            }
        }

    }
    /// <summary>
    /// ����������Ϣ����
    /// </summary>
    /// <param name="msg"></param>
    public override void ProcessEvent(MsgBase msg)
    {
        if (!eventTree.ContainsKey(msg.msgID))
        {
            Debug.LogError("msg id " + msg.msgID + " not exsits");
            Debug.LogError("msg Manager ==" + msg.GetManager());
        }
        else
        {
            EventNode tmp = eventTree[msg.msgID];
            do
            {
                tmp.data.ProcessEvent(msg);
                tmp = tmp.next;
            } while (tmp != null);

        }
    }

}
