using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using System.Threading;

namespace BoomboxController
{
    [BepInPlugin("KoderTech.BoomboxController", "BoomboxController", Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;

        public static Harmony HarmonyLib;

        public static Configs config;

        public static BoomboxController controller;

        public const string Version = "1.2.3";

        private void Awake()
        {
            instance = this;
            config = new Configs();
            controller = new BoomboxController();
            HarmonyLib = new Harmony("com.kodertech.BoomboxController");
            Startup();
        }

        public void WriteLogo()
        {
            string asciiArt = @"
            _________        .__               ________          ____ ___                       
            \_   ___ \_____  |__|__  ________  \______ \   ____ |    |   \_______  __________  
            /    \  \/\__  \ |  \  \/  /\__  \  |    |  \ /  _ \|    |   /\_  __ \/  ___/  _ \ 
            \     \____/ __ \|  |>    <  / __ \_|    `   (  <_> )    |  /  |  | \/\___ (  <_> )
             \______  (____  /__/__/\_ \(____  /_______  /\____/|______/   |__|  /____  >____/ 
                    \/     \/         \/     \/        \/                             \/       
            ";
            Logger.LogInfo(asciiArt);
        }

        public void Startup()
        {
            new WinApi().SizeConsole(1500, 500);
            WriteLogo();
            SwitchLanguage();
            if (File.Exists(@$"BoomboxController\lang\boombox_ru.cfg")) File.Delete(@$"BoomboxController\lang\boombox_ru.cfg");
            if (File.Exists(@$"BoomboxController\lang\boombox_en.cfg")) File.Delete(@$"BoomboxController\lang\boombox_en.cfg");
            if (!Directory.Exists(@$"BoomboxController\lang")) Directory.CreateDirectory(@$"BoomboxController\lang");
            if (!Directory.Exists(@$"BoomboxController\other")) Directory.CreateDirectory(@$"BoomboxController\other");
            if (!Directory.Exists(@$"BoomboxController\other\local")) Directory.CreateDirectory(@$"BoomboxController\other\local");
            if (!Directory.Exists(@$"BoomboxController\other\playlist")) Directory.CreateDirectory(@$"BoomboxController\other\playlist");
            if (!File.Exists(@$"BoomboxController\other\ffmpeg.exe"))
            {
                if (File.Exists(@$"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"))
                {
                    if (!Downloader.Unpacking())
                    {
                        Thread thread = new Thread(() => Downloader.DownloadFilesToUnpacking(new Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip"), @"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"));
                        thread.Start();
                    }
                }
                else
                {
                    Thread thread = new Thread(() => Downloader.DownloadFilesToUnpacking(new Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip"), @"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"));
                    thread.Start();
                }
            }
            controller.InitializationBoombox();
        }

        public void SwitchLanguage()
        {
            switch (config.languages.Value.ToLower())
            {
                case "ru":
                    config.GetLang().GetConfigRU();
                    break;
                case "en":
                    config.GetLang().GetConfigEN();
                    break;
            }
        }

        public void Log(object message)
        {
            Logger.LogInfo(message);
        }
    }
}
