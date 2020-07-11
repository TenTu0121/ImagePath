/* ***********************************************
* Discribe(说明)：监听与广播系统之关于事件管理的类
* Author(作者)：TenTu
* CreateTime(时间)：2020-03-24 14:41:12
* Email：1939093693@qq.com
* Copyright：@TenTu
* ************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理类
/// </summary>
public class EventCenter
{
    //创建字典Dictionary把{EventType}与{Delegate}对应起来
    private static Dictionary<EventType, Delegate> m_EventDic = new Dictionary<EventType, Delegate>();

    #region 添加监听的方法
    /// <summary>
    /// 添加监听的最终方法AddListener，就是说：关注是否有想接的任务发布
    /// </summary>
    /// <param name="eventType">其实就是{EventType}</param>
    /// <param name="callBack">其实就是{Delegate}}（Delegate封装成了CallBack类）</param>
    
    // no parameters
    public static void AddListener(EventType eventType, CallBack callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] + callBack;
    }
    // one parameters
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] + callBack;
    }
    // two parameters
    public static void AddListener<T,X>(EventType eventType, CallBack<T,X> callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack<T, X>)m_EventDic[eventType] + callBack;
    }
    // three parameters
    public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack<T, X, Y>)m_EventDic[eventType] + callBack;
    }
    // four parameters
    public static void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack<T, X, Y, Z>)m_EventDic[eventType] + callBack;
    }
    // five parameters
    public static void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        //调用设置监听之前的逻辑判断方法
        ListenerAdding(eventType, callBack);
        //因为是链式结构的的直接等于原来的加上新来的就行了 == 但是传入的EventType所对应的{CallBack}可能需要强转类型
        m_EventDic[eventType] = (CallBack<T, X, Y, Z, W>)m_EventDic[eventType] + callBack;
    }
    #endregion

    #region 移除监听的方法
    /// <summary>
    /// 移除监听的最终方法RemoveListener，就是说：完成任务的话，就不需要这个任务了，不然挂着不好
    /// </summary>
    /// <param name="eventType">其实就是{EventType}</param>
    /// <param name="callBack">其实就是{Delegate}}（Delegate封装成了CallBack类）</param>

    // no parameters
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }
    // one parameters
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }
    // two parameters
    public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack<T,X>)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }
    // three parameters
    public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack<T, X, Y>)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }
    // four parameters
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack<T, X, Y,Z>)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }
    // five parameters
    public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        //移除监听之前的逻辑判断方法
        ListenerRemoving(eventType, callBack);
        //解除关联，相当于移除eventType对应的委托中的传入的委托，不一定eventType就没有对应的委托了
        m_EventDic[eventType] = (CallBack<T, X, Y, Z, W>)m_EventDic[eventType] - callBack;
        //用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了
        ListenerRemoved(eventType);
    }

    #endregion

    #region 广播监听方法BroadCast，就是发布任务
    /// <summary>
    /// 广播监听方法BroadCast，就是发布任务
    /// </summary>
    /// <param name="eventType">	参数是EventType == 用于广播的事件码</param>

    // no parameters
    public static void BroadCast(EventType eventType)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack callBack = d as CallBack;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack();
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }
    // one parameters
    public static void BroadCast<T>(EventType eventType, T arg1)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack<T> callBack = d as CallBack<T>;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack(arg1);
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }
    // two parameters
    public static void BroadCast<T, X>(EventType eventType, T arg1, X arg2)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack<T, X> callBack = d as CallBack<T, X>;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack(arg1, arg2);
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }
    // three parameters
    public static void BroadCast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack(arg1, arg2, arg3);
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }
    // four parameters
    public static void BroadCast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack(arg1, arg2, arg3, arg4);
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }
    // five parameters
    public static void BroadCast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        //定义一个Delegate类型的d，用来接受{EventType}对应的委托
        //因为不管有没有参数，对应的委托都是Delegate类型的
        Delegate d;
        //通过从字典里取出这个{EventType}对应的委托
        //{ m_EventDic.TryGetValue(eventType, out d)}获取eventType对应的委托，返回值是bool，out给的是eventType对应的委托
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            //强转一下，转成需要的CallBack类型
            CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
            //看看强转之后是否是空的，不是空的就说明没问题，它们是一个CallBack类型，那就调用一下这个委托就行了呗
            if (callBack != null)
            {
                //直接调用就完事了，有参就写参，无参就无参
                callBack(arg1, arg2, arg3, arg4, arg5);
            }
            else
            {
                //若为空，就说明强转不了，强转不了就说明它们不是一个CallBack类型，如果有参的CallBack强转成无参的CallBack那么就会是空
                throw new Exception(string.Format("尝试广播事件错误：当前事件的事件码{0}对应的委托为不同的类型", eventType));
            }
        }
    }


    #endregion

    #region 设置监听之前的逻辑判断方法
    /// <summary>
    /// 添加监听方法AddListener == {public & staic}的第一步：设置监听之前的逻辑判断方法
    /// </summary>
    /// <param name="eventType">其实就是{EventType}</param>
    /// <param name="callBack">其实就是{Delegate}}</param>
    private static void ListenerAdding(EventType eventType, Delegate callBack)
    {
        //首先需要判断== 该字典里，传入的EventType是否已经存在。

        //m_EventDic.ContainsKey(eventType)返回值就是bool值类型
        if (!m_EventDic.ContainsKey(eventType))
        {
            //不存在则添加一下该EventType，并传一个null给它
            m_EventDic.Add(eventType, null);
        }

        Delegate d = m_EventDic[eventType];

        //再判断 == 传入的委托{CallBack} 类型 与传入的EventType所对应的{CallBack} 类型 是否一样
        if (d != null && d.GetType() != callBack.GetType())
        {
            //抛出一下异常，说明一下原因
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件的委托类型是{1}，而传入的委托类型是{2}",eventType, d.GetType(), callBack.GetType()));
        }
    }

    #endregion

    #region 移除监听之前的逻辑判断方法
    /// <summary>
    /// 移除监听方法RemoveListener == {public & staic}的第一步：移除监听之前的逻辑判断方法
    /// </summary>
    /// <param name="eventType">其实就是{EventType}</param>
    /// <param name="callBack">其实就是{Delegate}}</param>
    private static void ListenerRemoving(EventType eventType, Delegate callBack)
    {
        //首先需要判断== 该字典里，传入的EventType是否已经存在，若不存在else，则抛出异常

        //m_EventDic.ContainsKey(eventType)返回值就是bool值类型
        if (m_EventDic.ContainsKey(eventType))
        {
            //存在时，再判断，传入的EventType所对应的{CallBack}是否为空

            //错误：m_EventDic(eventType)，索引Key值是用中括号来括住的：m_EventDic[eventType]
            Delegate d = m_EventDic[eventType];
            if (d == null)
            {
                //传入的EventType所对应的{CallBack}为空，则抛出异常
                throw new Exception(string.Format("移除监听时发生错误，此事件码{0}对应的委托为空！",eventType));
            }
            //再判断 == 传入的委托{CallBack} 类型 与传入的EventType所对应的{CallBack} 类型 是否一样
            if (callBack.GetType() != d.GetType())
            {
                //不一样，则抛出异常
                throw new Exception(string.Format("移除监听时发生错误，尝试为当前事件{0}移除不同类型的委托，当前事件对应的委托类型是{1}，而需要移除的委托类型是{2}", eventType,d.GetType(),callBack.GetType()));
            }
        }
        else
        {
            //该字典里，传入的EventType不存在，则抛出异常
            throw new Exception(string.Format("移除监听时发生错误，此事件码{0}不存在！", eventType));
        }
        //经过上述逻辑判断后，基本没有问题，因此解除关联
    }

    /// <summary>
    /// 用于判断事件码对应的事件是否为空，若为空，那么这个事件码就可以移除了 remove
    /// </summary>
    /// <param name="eventType">需要判断对应的事件是否为空的事件码</param>
    private static void ListenerRemoved(EventType eventType)
    {
        //如果事件码对应的事件已经空了，那么这个事件码就可以移除了 remove
        if (m_EventDic[eventType] == null)
        {
            m_EventDic.Remove(eventType);
        }
    }

    #endregion
}
