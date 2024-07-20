using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using NewXp.IniApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDIrSys
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("存储路径")]
        public string Pach { get; set; } = "C:\\DIRSave";

    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "YF-OFFICE";
        public override Version Version => new Version(1, 0, 0);
        public override string Name => "Newdir";
        public Plugin plugin;
        public override void OnEnabled()
        {
            plugin = this;
            Log.Info("加载插件中");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            plugin = null;
            Log.Info("插件关闭了");
            base.OnDisabled();
        }
        //玩家加入时间
        public void Join(VerifiedEventArgs ev)
        {
            if (!File.Exists(Config.Pach+"\\"+ev.Player.UserId+".ini"))
            {
                IniFile iniFile = new IniFile(Config.Pach + "\\" + ev.Player.UserId + ".ini");
                iniFile.Section("DIR").Set("称号", "空", "称号");
                iniFile.Section("DIR").Set("称号颜色", "空");
                iniFile.Section("DIR").Set("管理权限组", "空", "默认分配的权限组");
                iniFile.Section("DIR").Set("QQ", "空");
                iniFile.Section("DIR").Set("等级", "否");
                iniFile.Section("DIR").Set("经验", "否", "是否给予管理");


            }
        
        }
    }
}
