using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
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
using Pl = Exiled.Events.Handlers.Player;
using SE = Exiled.Events.Handlers.Server;

namespace NDIrSys
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("存储路径")]
        public string Pach { get; set; } = "C:\\DIRSave";
        [Description("彩色称号更新频率")]
        public int each { get; set; } = 1;

    }
    public class Plugin : Plugin<Config>
    {
        public override string Author => "YF-OFFICE";
        public override Version Version => new Version(1, 2, 0);
        public override string Name => "Newdir";
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
        public override void OnEnabled()
        {
            plugin = this;
            
            if (!Directory.Exists(Config.Pach))
            {
                Directory.CreateDirectory(Config.Pach);
                Log.Warn("已创建存储文件夹" +Config.Pach);
            
            }
            Pl.Verified += this.Join;
            SE.WaitingForPlayers += this.wiat;
            Log.Info("bingo加载完成");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            plugin = null;
            Log.Info("插件关闭了");
            base.OnDisabled();
        }
        public void wiat()
        {
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
        public void Join(VerifiedEventArgs ev)
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
                        ev.Player.RankName = iniFile.Section("DIR").Get("称号");
                        switch (iniFile.Section("DIR").Get("称号颜色"))
                        {
                            case "rainbow":
                                rainbw.Add(ev.Player);
                                break;
                            default:
                                ev.Player.RankColor = iniFile.Section("DIR").Get("称号颜色");
                                break;
                        }
                    }

                }
                else
                {
                    Server.ExecuteCommand($"/setgroup {ev.Player.Id} {iniFile.Section("DIR").Get("管理权限组")}");
                    Log.Info($"已经给予{ev.Player.Nickname}-{ev.Player.UserId}==={iniFile.Section("DIR").Get("管理权限组")}权限");
                    if (iniFile.Section("DIR").Get("称号") != "空")
                    {
                        ev.Player.RankName = iniFile.Section("DIR").Get("称号");
                        switch (iniFile.Section("DIR").Get("称号颜色"))
                        {
                            case "rainbow":
                                rainbw.Add(ev.Player);
                                break;
                            default:
                                ev.Player.RankColor = iniFile.Section("DIR").Get("称号颜色");
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
                    item.RankColor = FMoreColo.GetRandomValue();
                }
                yield return Timing.WaitForSeconds(Config.each);
            }
        
        }
    }
}
