﻿using Native.Csharp.App;
using Native.Csharp.App.Config;
using Native.Csharp.App.Interface;
using Native.Csharp.App.Model;
using Native.Csharp.Sdk.Cqp.Api;
using Native.Csharp.Sdk.Cqp.Enum;
using Native.Csharp.Sdk.Cqp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Native.Csharp.Business
{
    public class GroupManager : IGroupManager
    {
        IXuanShang _xuanShang;
        IConfig _config;
        IJingCaiChaXun _jingCaiChaXun;
        public GroupManager(IConfig config, IXuanShang xuanShang, IJingCaiChaXun jingCaiChaXun)
        {
            _config = config;
            _xuanShang = xuanShang;
            _jingCaiChaXun = jingCaiChaXun;
        }
        /// <summary>
		/// Type=302 群事件 - 群请求 - 申请入群<para/>
		/// 处理收到的群请求 (申请入群) 事件
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
        public void ReceiveGroupAddApply(object sender, GroupAddRequestEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupAddInvitee(object sender, GroupAddRequestEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupFileUpload(object sender, FileUploadMessageEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupManageDecrease(object sender, GroupManageAlterEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupManageIncrease(object sender, GroupManageAlterEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupMemberInvitee(object sender, GroupMemberAlterEventArgs e)
        {
            e.Handled = false;
        }
        /// <summary>
		/// Type=103 群事件 - 群成员增加 - 主动入群<para/>
		/// 处理收到的群成员增加 (主动入群) 事件
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
        public void ReceiveGroupMemberJoin(object sender, GroupMemberAlterEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupMemberLeave(object sender, GroupMemberAlterEventArgs e)
        {
            e.Handled = false;
        }

        public void ReceiveGroupMemberRemove(object sender, GroupMemberAlterEventArgs e)
        {
            e.Handled = false;
        }
        /// <summary>
		/// Type=2 群消息<para/>
		/// 当在派生类中重写时, 处理收到的群消息
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
        public void ReceiveGroupMessage(object sender, GroupMessageEventArgs e)
        {
            e.Handled = false;
            var config = _config.Get();
            if (config == null || config.groupIds.Count(n => n == e.FromGroup) <= 0)
                return;
            if (string.IsNullOrEmpty(e.Msg) || (e.Msg.IndexOf(config.cmdPrefix) < 0 && e.Msg.IndexOf(config.jingCaiCmdPrefix)<0))
                return;
            string msg = string.Empty;
            if (e.Msg.Substring(0, config.cmdPrefix.Length).ToLower() == config.cmdPrefix.ToLower())
            {
                try
                {
                    msg = _xuanShang.Get(e.Msg.Replace(config.cmdPrefix, ""));
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

            }
            else if(e.Msg.Substring(0, config.jingCaiCmdPrefix.Length).ToLower() == config.jingCaiCmdPrefix.ToLower()) {
                try
                {
                    msg = _jingCaiChaXun.GetMsg();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            if (!string.IsNullOrEmpty(msg))
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, Common.CqApi.CqCode_At(e.FromQQ) + "\r\n" + msg);
            }
            e.Handled = true;
        }

        public void ReceiveGroupPrivateMessage(object sender, PrivateMessageEventArgs e)
        {
            e.Handled = false;
        }
    }
}