using Scripter.Properties;

namespace Scripter
{
    public class Config
    {
        private static Config _instance;

        private Config()
        {
            sendMode = Settings.Default.SendMode;
            sendKeyUp = Settings.Default.SendKeyUp;
            pauseAfterSend = Settings.Default.PauseAfterSend;
        }
        public static Config Instance
        {
            get { if (_instance == null) _instance = new Config();
                return _instance; }
        }

        private bool sendMode;
        private bool sendKeyUp;
        private int pauseAfterSend;

        public bool SendMode
        {
            get { return sendMode; }
            set { sendMode = value;
            Settings.Default.SendMode = value; Settings.Default.Save();
            }
        }

        public bool SendKeyUp
        {
            get { return sendKeyUp; }
            set { sendKeyUp = value;
            Settings.Default.SendKeyUp = value; Settings.Default.Save();
            }
        }

        public int PauseAfterSend
        {
            get { return pauseAfterSend; }
            set { pauseAfterSend = value; Settings.Default.PauseAfterSend = value; Settings.Default.Save(); }
        }
    }
}