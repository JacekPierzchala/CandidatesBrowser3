using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class Attachment : INotifyPropertyChanged
    {
        private string path;
        public string Path
        {
            get { return path; }
            set {
                path = value;
                RaisePropertyChange("Path");
                }
        }

        public Attachment(string path)
        {
            Path = path;
        }
        

        public void RaisePropertyChange(string propertyName)
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
