using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class ConfigProjectCandidate : INotifyPropertyChanged, ICloneable
    {
        private int configProjectID;
        public int ConfigProjectID
        {
            get { return configProjectID; }
            set {
                configProjectID = value;
                RaisePropertyChanged("ConfigProjectID");
                }
        }

        private int configAreaID;
        public int ConfigAreaID
        {
            get { return configAreaID; }
            set
            {
                configAreaID = value;
                RaisePropertyChanged("ConfigAreaID");
            }
        }

        private int configProjectLibID;
        public int ConfigProjectLibID
        {
            get { return configProjectLibID; }
            set
            {
                configProjectLibID = value;
                RaisePropertyChanged("ConfigProjectLibID");
            }
        }

        private int configGroupID;
        public int ConfigGroupID
        {
            get { return configGroupID; }
            set
            {
                configGroupID = value;
                RaisePropertyChanged("ConfigGroupID");
            }
        }

        private int configCandidateID;
        public int ConfigCandidateID
        {
            get { return configCandidateID; }
            set
            {
                configCandidateID = value;
                RaisePropertyChanged("ConfigCandidateID");
            }
        }

        public ConfigProjectCandidate(DataRow row)
        {
            ConfigProjectID = row.Field<int>("CONFIG_PROJECT_ID");
            ConfigAreaID = row.Field<int>("CONFIG_AREA_ID");
            ConfigProjectLibID = row.Field<int>("CONFIG_PROJECT_LIB_ID");
            ConfigGroupID = row.Field<int>("CONFIG_GROUP_ID");
            ConfigCandidateID = row.Field<int>("CANDIDATES_ID");
            
                
                
        }

        public ConfigProjectCandidate()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
