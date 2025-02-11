using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;
using NewXp.IniApi;
using PlayerRoles.Voice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NDIrSys
{
    public class Config
    {
        [Description("存储路径")]
        public string Pach { get; set; } = "C:\\DIRSave";
        [Description("彩色称号更新频率")]
        public int each { get; set; } = 1;

    }
    public class Plugin1 : Plugin<Config>
    {
        public override string Author => "YF-OFFICE";
        public override Version Version => new Version(1, 0, 0);
        public override string Name => "Newdir";

        public override string Description => "权限和称号简化给予系统 dirsystem";

        public override Version RequiredApiVersion => new Version(LabApiProperties.CompiledVersion);
        public Plugin plugin;
        public static List<Player> rainbw = new List<Player>();
        public static string[] FMoreColo = new string[]
        {
            "pink",
            "silver",
            "cyan",
            "aqua",
            "tomato",
            "yellow",
            "magenta",
            "orange",
            "lime",
            "green",
            "red",
            "brown",
            "red",
                "orange",
                "yellow",
                "green",
                "blue_green",
                "magenta",
                "pink",
                "brown",
                "silver",
             "light_green",
           "crimson",
         "cyan",
       "aqua",
           "deep_pink",
                     "tomato",
                             "blue_green",
                                         "lime",
                                 "emerald",
                         "carmine",
                   "nickel",
              "mint",
    "army_green",
                                   "pumpkin"
        };
        public static CoroutineHandle Handle = new CoroutineHandle();
        public override void Enable()
        {
            plugin = this;
            rainbw.Clear();
            if (!Directory.Exists(Config.Pach))
            {
                Directory.CreateDirectory(Config.Pach);
                Logger.Warn("已创建存储文件夹" +Config.Pach);
            
            }
            PlayerEvents.Joined += this.Join;
            PlayerEvents.Left += this.Leave;
            ServerEvents.WaitingForPlayers += this.wiat;
            Logger.Info("bingo加载完成");
        }
        public override void Disable()
        {
            plugin = null;
            PlayerEvents.Joined -= this.Join;
            PlayerEvents.Left -= this.Leave;
            ServerEvents.WaitingForPlayers -= this.wiat;
            Logger.Info("插件关闭了");
        }
        public void wiat()
        {
              rainbw.Clear();
            if (Handle.IsRunning)
            {
                Timing.KillCoroutines(Handle);
                Handle = Timing.RunCoroutine(Rainbw());
            }
            else
            {
                Handle = Timing.RunCoroutine(Rainbw());
            }
        }
        public void Leave(PlayerLeftEventArgs ev)
        {
            if (rainbw.Contains(ev.Player))
            { 
             rainbw.Remove(ev.Player);
            }
        }
        public void Join(PlayerJoinedEventArgs ev)
        {
          
            if (!File.Exists(Config.Pach + "\\" + ev.Player.UserId + ".ini"))
            {
                IniFile iniFile = new IniFile();
                iniFile.Section("DIR").Set("称号", "空", "称号");
                iniFile.Section("DIR").Set("称号颜色", "空");
                iniFile.Section("DIR").Set("管理权限组", "空");
                iniFile.Save(Config.Pach + "\\" + ev.Player.UserId + ".ini");
            }
            else
            {
                
                IniFile iniFile = new IniFile(Config.Pach + "\\" + ev.Player.UserId + ".ini");
                iniFile.Save(Config.Pach + "\\" + ev.Player.UserId + ".ini");
                if (iniFile.Section("DIR").Get("管理权限组") == "空")
                {
                    if (iniFile.Section("DIR").Get("称号") != "空")
                    {
                        ev.Player.GroupName = iniFile.Section("DIR").Get("称号");
                        switch (iniFile.Section("DIR").Get("称号颜色"))
                        {
                            case "rainbow":
                                rainbw.Add(ev.Player);
                                break;
                            default:
                                ev.Player.GroupColor = iniFile.Section("DIR").Get("称号颜色");
                                break;
                        }
                    }

                }
                else
                {
                    Server.RunCommand($"/setgroup {ev.Player.PlayerId} {iniFile.Section("DIR").Get("管理权限组")}");
                    Logger.Info($"已经给予{ev.Player.Nickname}-{ev.Player.UserId}==={iniFile.Section("DIR").Get("管理权限组")}权限");
                    if (iniFile.Section("DIR").Get("称号") != "空")
                    {
                        ev.Player.GroupName = iniFile.Section("DIR").Get("称号");
                        switch (iniFile.Section("DIR").Get("称号颜色"))
                        {
                            case "rainbow":
                                rainbw.Add(ev.Player);
                                break;
                            default:
                                ev.Player.GroupColor = iniFile.Section("DIR").Get("称号颜色");
                                break;
                        }
                    }
                }
            }

        
        }
        public IEnumerator<float> Rainbw()
        {
            while (true)
            {
                foreach (var item in rainbw)
                {
                    item.GroupColor = FMoreColo.RandomItem();
                }
                yield return Timing.WaitForSeconds(Config.each);
            }
        
        }
    }
}
