using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public enum ActionType
    {
        Save,Delete
    }
    public class Document: INotifyPropertyChanged
    {
        private string[] documentNames;
        public string[] DocumentNames
        {
            get { return documentNames; }
            set {
                documentNames = value;
                RaisePropertyChange("DocumentName");
                }
        }

        private ActionType action;
        public ActionType Action
        {
            get { return action; }
            set { action = value; }
        }

        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }



        public  void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public  object Clone()
        {
            return this.MemberwiseClone();
        }

        public  event PropertyChangedEventHandler PropertyChanged;
    }
}
