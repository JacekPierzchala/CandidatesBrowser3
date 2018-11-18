using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class ConfigProjectsLib: INotifyPropertyChanged, ICloneable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set {
                id = value;
                RaisePropertyChanged("Id");
                }
        }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set {
                projectName = value;
                RaisePropertyChanged("ProjectName");
                }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        public ConfigProjectsLib()
        {
            Selected = true;
        }
        public ConfigProjectsLib(DataRow row):this()
        {
            Id = row.Field<int>("ID");
            ProjectName = row.Field<string>("PROJECT_NAME");
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged!=null)
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
