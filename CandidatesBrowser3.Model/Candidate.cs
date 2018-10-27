using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class Candidate : INotifyPropertyChanged
    {
        private int id;
        public int ID
        {
            get { return id; }
            set {
                id = value;
                RaisePropertyChange("ID");
                }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                RaisePropertyChange("FirstName");
            }
        }

        public void RaisePropertyChange(string propertyName)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
