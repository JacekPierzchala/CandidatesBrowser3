using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Utilities
{
    public class ConfigProjectMessenger
    {
        private static ConfigProjectMessenger _instance;
        public static ConfigProjectMessenger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigProjectMessenger();
                }
                return _instance;
            }
        }

        public void RaiseConfigProjectChanged(ConfigProject configProject)
        {
            if (ConfigProjectChanged!=null)
            {
                ConfigProjectChanged.Invoke(this, new ConfigProjectValueChangedEventArgs() { ConfigProject=configProject });
            }
        }

        public event EventHandler<ConfigProjectValueChangedEventArgs> ConfigProjectChanged;
    }

    public class ConfigProjectValueChangedEventArgs : EventArgs
    {
        public ConfigProject ConfigProject { get; set; }
    }

    
}
