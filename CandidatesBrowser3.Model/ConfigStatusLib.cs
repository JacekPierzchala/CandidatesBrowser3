using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class ConfigStatusLib : INotifyPropertyChanged, ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set {
                id = value;
                RaisePropertyChanged("ID");
                }
        }

        private string definition;
        public string Definition
        {
            get { return definition; }
            set {
                definition = value;
                RaisePropertyChanged("Definition");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set {
                description = value;
                RaisePropertyChanged("Description");
                }
        }

        public ConfigStatusLib()
        {

        }

        public ConfigStatusLib(DataRow dr)
        {
            ID = dr.Field<int>("ID");
            Description = dr.Field<string>("DESCRIPTION");
            Definition = dr.Field<string>("DEFINITION");
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
