﻿/*
 *	此代码由模板生成, 请勿随意修改此源代码, 防止出现错误
 *	需要更新 AppID 请右击模板文件, 点击运行自定义工具
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Unity;
using Native.Csharp.App.Event;
using Native.Csharp.App.Model;
using Native.Csharp.App.Interface;
using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.Enum;
using Native.Csharp.Sdk.Cqp.Other;
using Native.Csharp.Repair;

namespace Native.Csharp.App.Core
{
	public class LibExport
	{
		#region --字段--
		private static Encoding _defaultEncoding = null;
		#endregion

		#region --构造函数--
		/// <summary>
		/// 静态构造函数, 注册依赖注入回调
		/// </summary>
		/// <returns></returns>
		static LibExport ()
		{
			_defaultEncoding = Encoding.GetEncoding ("GB18030");
			
			// 初始化 Costura
			CosturaUtility.Initialize ();

			// 初始化依赖注入容器
			Common.UnityContainer = new UnityContainer ();

			// 程序开始调用方法进行注册
			Event_AppMain.Registbackcall (Common.UnityContainer);

			// 注册完毕调用方法进行分发
			Event_AppMain.Resolvebackcall (Common.UnityContainer);

			// 分发应用内注册事件
			ResolveAppbackcall ();
		}
		#endregion

		#region --核心方法--
		/// <summary>
		/// 返回 AppID 与 ApiVer, 本方法在模板运行后会根据项目名称自动填写 AppID 与 ApiVer
		/// </summary>
		/// <returns></returns>
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]
		private static string AppInfo ()
		{
			// 请勿随意修改
			//
			// 当前项目名称: Native.Csharp
			// Api版本: 9

			return string.Format ("{0},{1}", 9, "Native.Csharp");
		}

		/// <summary>
		/// 接收插件 AutoCode, 注册异常
		/// </summary>
		/// <param name="authCode"></param>
		/// <returns></returns>
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]
		private static int Initialize (int authCode)
		{
			// 酷Q获取应用信息后，如果接受该应用，将会调用这个函数并传递AuthCode。
			Common.CqApi = new CqApi (authCode);

			// AuthCode 传递完毕后将对象加入容器托管, 以便在其它项目中调用
			Common.UnityContainer.RegisterInstance<CqApi> (Common.CqApi);

			// 注册插件全局异常捕获回调, 用于捕获未处理的异常, 回弹给 酷Q 做处理
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			// 本函数【禁止】处理其他任何代码，以免发生异常情况。如需执行初始化代码请在Startup事件中执行（Type=1001）。
			return 0;
		}
		#endregion

		#region --私有方法--
		/// <summary>
		/// 获取所有的注入项, 分发到对应的事件
		/// </summary>
		private static void ResolveAppbackcall ()
		{
			#region --IEvent_AppStatus--
			foreach (var instance in Common.UnityContainer.ResolveAll<ICqStartup> ())
			{
				LibExport.CqStartup += instance.CqStartup;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<ICqExit> ())
			{
				LibExport.CqExit += instance.CqExit;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IAppEnable> ())
			{
				LibExport.AppEnable += instance.AppEnable;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IAppDisable> ())
			{
				LibExport.AppDisable += instance.AppDisable;
			}
			#endregion

			#region --IEvent_DiscussMessage--
			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveDiscussMessage> ())
			{
				LibExport.ReceiveDiscussMessage += instance.ReceiveDiscussMessage;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveDiscussPrivateMessage> ())
			{
				LibExport.ReceiveDiscussPrivateMessage += instance.ReceiveDiscussPrivateMessage;
			}
			#endregion

			#region --IEvent_FriendMessage--
			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveFriendAddRequest> ())
			{
				LibExport.ReceiveFriendAdd += instance.ReceiveFriendAddRequest;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveFriendIncrease> ())
			{
				LibExport.ReceiveFriendIncrease += instance.ReceiveFriendIncrease;
			}

			foreach (var instace in Common.UnityContainer.ResolveAll<IReceiveFriendMessage> ())
			{
				LibExport.ReceiveFriendMessage += instace.ReceiveFriendMessage;
			}
			#endregion

			#region --IEvent_GroupMessage--
			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupMessage> ())
			{
				LibExport.ReceiveGroupMessage += instance.ReceiveGroupMessage;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupPrivateMessage> ())
			{
				LibExport.ReceiveGroupPrivateMessage += instance.ReceiveGroupPrivateMessage;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupFileUpload> ())
			{
				LibExport.ReceiveFileUploadMessage += instance.ReceiveGroupFileUpload;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupManageIncrease> ())
			{
				LibExport.ReceiveManageIncrease += instance.ReceiveGroupManageIncrease;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupManageDecrease> ())
			{
				LibExport.ReceiveManageDecrease += instance.ReceiveGroupManageDecrease;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupMemberPass> ())
			{
				LibExport.ReceiveMemberJoin += instance.ReceiveGroupMemberJoin;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupMemberIncrease> ())
			{
				LibExport.ReceiveMemberInvitee += instance.ReceiveGroupMemberInvitee;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupMemberLeave> ())
			{
				LibExport.ReceiveMemberLeave += instance.ReceiveGroupMemberLeave;
			}

			foreach (var instance in Common.UnityContainer.ResolveAll<IReceiveGroupMemberRemove> ())
			{
				LibExport.ReceiveMemberRemove += instance.ReceiveGroupMemberRemove;
			}

			foreach (var instaice in Common.UnityContainer.ResolveAll<IReceiveAddGroupRequest> ())
			{
				LibExport.ReceiveGroupAddApply += instaice.ReceiveGroupAddApply;
			}

			foreach (var instaice in Common.UnityContainer.ResolveAll<IReceiveAddGroupBeInvitee> ())
			{
				LibExport.ReceiveGroupAddInvitee += instaice.ReceiveGroupAddInvitee;
			}
			#endregion

			#region --IEvent_OtherMessage--
			foreach (var otherMessage in Common.UnityContainer.ResolveAll<IReceiveOnlineStatusMessage> ())
			{
				LibExport.ReceiveOnlineStatusMessage += otherMessage.ReceiveOnlineStatusMessage;
			}
			#endregion
		}

		/// <summary>
		/// 全局异常捕获, 用于捕获开发者未处理的异常, 此异常将回弹至酷Q进行处理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				StringBuilder innerLog = new StringBuilder ();
				innerLog.AppendLine ("发现未处理的异常!");
				innerLog.AppendLine ("异常堆栈：");
				innerLog.AppendLine (ex.ToString ());
				Common.CqApi.AddFatalError (innerLog.ToString ());      //将未经处理的异常弹回酷Q做处理
			}
		}
		#endregion

		#region --回调事件--
		/// <summary>
		/// 酷Q事件: _eventStartup 回调
		/// <para>Type=1001 酷Q启动</para>
		/// </summary>
		public static event EventHandler<EventArgs> CqStartup = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventExit
		/// <para>Type=1002 酷Q退出</para>
		/// </summary>
		public static event EventHandler<EventArgs> CqExit = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventEnable
		/// <para>Type=1003 应用已被启用</para>
		/// </summary>
		public static event EventHandler<EventArgs> AppEnable = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventDisable
		/// <para>Type=1004 应用将被停用</para>
		/// </summary>
		public static event EventHandler<EventArgs> AppDisable = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventPrivateMsg
		/// <para>Type=21 私聊消息 - 好友</para>
		/// </summary>
		public static event EventHandler<PrivateMessageEventArgs> ReceiveFriendMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventPrivateMsg
		/// <para>Type=21 私聊消息 - 在线状态</para>
		/// </summary>
		public static event EventHandler<PrivateMessageEventArgs> ReceiveOnlineStatusMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventPrivateMsg
		/// <para>Type=21 私聊消息 - 群私聊</para>
		/// </summary>
		public static event EventHandler<PrivateMessageEventArgs> ReceiveGroupPrivateMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventPrivateMsg
		/// <para>Type=21 私聊消息 - 讨论组私聊</para>
		/// </summary>
		public static event EventHandler<PrivateMessageEventArgs> ReceiveDiscussPrivateMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventGroupMsg
		/// <para>Type=2 群消息</para>
		/// </summary>
		public static event EventHandler<GroupMessageEventArgs> ReceiveGroupMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventDiscussMsg
		/// <para>Type=4 讨论组消息</para>
		/// </summary>
		public static event EventHandler<DiscussMessageEventArgs> ReceiveDiscussMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventGroupUpload
		/// <para>Type=11 群文件上传事件</para>
		/// </summary>
		public static event EventHandler<FileUploadMessageEventArgs> ReceiveFileUploadMessage = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupAdmin
		/// <para>Type=101 群事件-管理员变动 - 群管理增加</para>
		/// </summary>
		public static event EventHandler<GroupManageAlterEventArgs> ReceiveManageIncrease = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupAdmin
		/// <para>Type=101 群事件-管理员变动 - 群管理减少</para>
		/// </summary>
		public static event EventHandler<GroupManageAlterEventArgs> ReceiveManageDecrease = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupMemberIncrease
		/// <para>Type=103 群事件-群成员增加 - 主动离开</para>
		/// </summary>
		public static event EventHandler<GroupMemberAlterEventArgs> ReceiveMemberLeave = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupMemberIncrease
		/// <para>Type=103 群事件-群成员增加 - 成员移除</para>
		/// </summary>
		public static event EventHandler<GroupMemberAlterEventArgs> ReceiveMemberRemove = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupMemberIncrease
		/// <para>Type=103 群事件-群成员增加 - 主动加群</para>
		/// </summary>
		public static event EventHandler<GroupMemberAlterEventArgs> ReceiveMemberJoin = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventSystem_GroupMemberIncrease
		/// <para>Type=103 群事件-群成员增加 - 邀请入群</para>
		/// </summary>
		public static event EventHandler<GroupMemberAlterEventArgs> ReceiveMemberInvitee = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventFriend_Add
		/// <para>Type=201 好友事件-好友已添加</para>
		/// </summary>
		public static event EventHandler<FriendIncreaseEventArgs> ReceiveFriendIncrease = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventRequest_AddFriend
		/// <para>Type=301 请求-好友添加</para>
		/// </summary>
		public static event EventHandler<FriendAddRequestEventArgs> ReceiveFriendAdd = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventRequest_AddGroup
		/// <para>Type=302 请求-群添加 - 申请入群</para>
		/// </summary>
		public static event EventHandler<GroupAddRequestEventArgs> ReceiveGroupAddApply = (sender, e) => { };

		/// <summary>
		/// 酷Q事件: _eventRequest_AddGroup
		/// <para>Type=302 请求-群添加 - 被邀入群</para>
		/// </summary>
		public static event EventHandler<GroupAddRequestEventArgs> ReceiveGroupAddInvitee = (sender, e) => { };
		#endregion

		#region --导出方法--
		[DllExport (ExportName = "_eventStartup", CallingConvention = CallingConvention.StdCall)]
		private static int EventStartUp ()
		{
			CqStartup (null, new EventArgs ());
			return 0;
		}

		[DllExport (ExportName = "_eventExit", CallingConvention = CallingConvention.StdCall)]
		private static int EventExit ()
		{
			CqExit (null, new EventArgs ());
			return 0;
		}

		[DllExport (ExportName = "_eventEnable", CallingConvention = CallingConvention.StdCall)]
		private static int EventEnable ()
		{
			AppEnable (null, new EventArgs ());
			return 0;
		}

		[DllExport (ExportName = "_eventDisable", CallingConvention = CallingConvention.StdCall)]
		private static int EventDisable ()
		{
			AppDisable (null, new EventArgs ());
			return 0;
		}

		[DllExport (ExportName = "_eventPrivateMsg", CallingConvention = CallingConvention.StdCall)]
		private static int EventPrivateMsg (int subType, int msgId, long fromQQ, IntPtr msg, int font)
		{
			PrivateMessageEventArgs args = new PrivateMessageEventArgs ();
			args.MsgId = msgId;
			args.FromQQ = fromQQ;
			args.Msg = msg.ToString (_defaultEncoding);
			args.Handled = false;

			if (subType == 11)      // 来自好友
			{
				ReceiveFriendMessage (null, args);
			}
			else if (subType == 1)  // 来自在线状态
			{
				ReceiveOnlineStatusMessage (null, args);
			}
			else if (subType == 2)  // 来自群
			{
				ReceiveGroupPrivateMessage (null, args);
			}
			else if (subType == 3)  // 来自讨论组
			{
				ReceiveDiscussPrivateMessage (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventPrivateMsg 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]
		private static int EventGroupMsg (int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font)
		{
			GroupMessageEventArgs args = new GroupMessageEventArgs ();
			args.MsgId = msgId;
			args.FromGroup = fromGroup;
			args.FromQQ = fromQQ;
			args.Msg = msg.ToString (_defaultEncoding);
			args.FromAnonymous = null;
			args.IsAnonymousMsg = false;
			args.Handled = false;

			if (fromQQ == 80000000 && !string.IsNullOrEmpty (fromAnonymous))
			{
				args.FromAnonymous = Common.CqApi.GetAnonymous (fromAnonymous); //获取匿名成员信息
				args.IsAnonymousMsg = true;
			}

			if (subType == 1)   // 群消息
			{
				ReceiveGroupMessage (null, args);
			}
			else                // 其它类型
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventGroupMsg 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventDiscussMsg", CallingConvention = CallingConvention.StdCall)]
		private static int EventDiscussMsg (int subType, int msgId, long fromDiscuss, long fromQQ, IntPtr msg, int font)
		{
			DiscussMessageEventArgs args = new DiscussMessageEventArgs ();
			args.MsgId = msgId;
			args.FromDiscuss = fromDiscuss;
			args.FromQQ = fromQQ;
			args.Msg = msg.ToString (_defaultEncoding);
			args.Handled = false;

			if (subType == 1)   // 讨论组消息
			{
				ReceiveDiscussMessage (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventDiscussMsg 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventGroupUpload", CallingConvention = CallingConvention.StdCall)]
		private static int EventGroupUpload (int subType, int sendTime, long fromGroup, long fromQQ, string file)
		{
			FileUploadMessageEventArgs args = new FileUploadMessageEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromGroup = fromGroup;
			args.FromQQ = fromQQ;
			args.File = Common.CqApi.GetFile (file);
			ReceiveFileUploadMessage (null, args);
			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventSystem_GroupAdmin", CallingConvention = CallingConvention.StdCall)]
		private static int EventSystemGroupAdmin (int subType, int sendTime, long fromGroup, long beingOperateQQ)
		{
			GroupManageAlterEventArgs args = new GroupManageAlterEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromGroup = fromGroup;
			args.BeingOperateQQ = beingOperateQQ;
			args.Handled = false;

			if (subType == 1)       // 被取消管理员
			{
				ReceiveManageDecrease (null, args);
			}
			else if (subType == 2)  // 被设置管理员
			{
				ReceiveManageIncrease (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventSystemGroupAdmin 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventSystem_GroupMemberDecrease", CallingConvention = CallingConvention.StdCall)]
		private static int EventSystemGroupMemberDecrease (int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
		{
			GroupMemberAlterEventArgs args = new GroupMemberAlterEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromGroup = fromGroup;
			args.FromQQ = fromQQ;
			args.BeingOperateQQ = beingOperateQQ;
			args.Handled = false;

			if (subType == 1)       // 群员离开
			{
				args.FromQQ = beingOperateQQ;   // 此时 FormQQ 为操作者QQ
				ReceiveMemberLeave (null, args);
			}
			else if (subType == 2)
			{
				ReceiveMemberRemove (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventSystemGroupMemberDecrease 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventSystem_GroupMemberIncrease", CallingConvention = CallingConvention.StdCall)]
		private static int EventSystemGroupMemberIncrease (int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
		{
			GroupMemberAlterEventArgs args = new GroupMemberAlterEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromGroup = fromGroup;
			args.FromQQ = fromQQ;
			args.BeingOperateQQ = beingOperateQQ;
			args.Handled = false;

			if (subType == 1)       // 管理员同意
			{
				ReceiveMemberJoin (null, args);
			}
			else if (subType == 2)  // 管理员邀请
			{
				ReceiveMemberInvitee (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventSystemGroupMemberIncrease 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventFriend_Add", CallingConvention = CallingConvention.StdCall)]
		private static int EventFriendAdd (int subType, int sendTime, long fromQQ)
		{
			FriendIncreaseEventArgs args = new FriendIncreaseEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromQQ = fromQQ;
			args.Handled = false;

			if (subType == 1)   // 好友已添加
			{
				ReceiveFriendIncrease (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventFriendAdd 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventRequest_AddFriend", CallingConvention = CallingConvention.StdCall)]
		private static int EventRequestAddFriend (int subType, int sendTime, long fromQQ, IntPtr msg, string responseFlag)
		{
			FriendAddRequestEventArgs args = new FriendAddRequestEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromQQ = fromQQ;
			args.AppendMsg = msg.ToString (_defaultEncoding);
			args.Tag = responseFlag;
			args.Handled = false;

			if (subType == 1)   // 好友添加请求
			{
				ReceiveFriendAdd (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventRequestAddFriend 方法发现新的消息类型");
			}

			return (int)(args.Handled ? MessageHanding.Intercept : MessageHanding.Ignored); //如果处理过就截断消息
		}

		[DllExport (ExportName = "_eventRequest_AddGroup", CallingConvention = CallingConvention.StdCall)]
		private static int EventRequestAddGroup (int subType, int sendTime, long fromGroup, long fromQQ, IntPtr msg, string responseFlag)
		{
			GroupAddRequestEventArgs args = new GroupAddRequestEventArgs ();
			args.SendTime = sendTime.ToDateTime ();
			args.FromGroup = fromGroup;
			args.FromQQ = fromQQ;
			args.AppendMsg = msg.ToString (_defaultEncoding);
			args.Tag = responseFlag;
			args.Handled = false;

			if (subType == 1)       // 申请加入群
			{
				ReceiveGroupAddApply (null, args);
			}
			else if (subType == 2)  // 机器人被邀请
			{
				ReceiveGroupAddInvitee (null, args);
			}
			else
			{
				Common.CqApi.AddLoger (LogerLevel.Info, "Native提示", "EventRequestAddGroup 方法发现新的消息类型");
			}

			return 0;
		}
		#endregion
	}
}