using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

namespace CandidatesBrowser3.Model
{
    public class ConfigProject : ConfigProjectsLib,INotifyPropertyChanged, ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged("ID");
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

        public string AreaName { get; set; }

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

        public ConfigProject(DataRow row)
        {
            ID = row.Field<int>("ID");
            ConfigAreaID = row.Field<int>("CONFIG_AREA_ID");
            AreaName = row.Field<string>("AREA_NAME");
            ConfigProjectLibID = row.Field<int>("CONFIG_PROJECT_LIB_ID");
            ConfigGroupID = row.Field<int>("CONFIG_GROUP_ID");
            ProjectName = row.Field<string>("PROJECT_NAME");
    
        }
        public ConfigProject()
        {

        }


        public new event PropertyChangedEventHandler PropertyChanged;
        public new void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public new object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
