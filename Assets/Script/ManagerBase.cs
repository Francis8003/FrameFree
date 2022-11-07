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
    //存储注册消息队列
    public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono">要注册的脚本</param>
    /// <param name="msgs">一个脚本可以注册多个msg</param>
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
    /// <param name="mono">注册的时间列表</param>
    public void RegistMsg(ushort id, EventNode node)
    {
        //如果数据链路里没有这个消息ID
        if (!eventTree.ContainsKey(id))
        {
            eventTree.Add(id, node);
        }
        //如果有这个消息ID，则在消息的末尾进行添加
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
    /// 注销多个消息
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
    /// 注销一个消息节点
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
        else//存在这个消息
        {
            EventNode tmp = eventTree[id];
            //要注销的节点是第一个节点
            if (node == tmp.data)
            {
                EventNode header = tmp;

                //除了第一个节点后边还有消息
                if (header.next != null)
                {
                    header.data = tmp.next.data;
                    header.next = tmp.next.next;
                }
                //只有第一个节点
                else
                {
                    eventTree.Remove(id);
                }
            }
            else//要注销的节点是中间或者尾部的消息 
            {
                while (tmp.next != null && tmp.next.data != null)//TBD
                {
                    tmp = tmp.next;
                }
                //去掉中间的节点
                if (tmp.next.next != null)
                {
                    tmp.next = tmp.next.next;
                }
                //去掉尾部的节点
                else
                {
                    tmp.next = null;
                }
            }
        }

    }
    /// <summary>
    /// 处理整个消息队列
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
