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
