/* ***********************************************
* Discribe(说明)：监听与广播系统之关于封装委托的类
* Author(作者)：TenTu
* CreateTime(时间)：2020-03-24 14:42:12
* Email：1939093693@qq.com
* Copyright：@TenTu
* ************************************************/
/// <summary>
/// 封装委托类：后期可拓展
/// </summary>
//无参数的委托
public delegate void CallBack();
//一个参数的委托
public delegate void CallBack<T>(T arg);
//两个参数的委托
public delegate void CallBack<T, X>(T arg1, X arg2);
//三个参数的委托
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
//四个参数的委托
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
//五个参数的委托
public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
