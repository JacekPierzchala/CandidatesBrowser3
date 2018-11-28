using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class ConfigCompany : INotifyPropertyChanged, ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        private string company;
        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        public ConfigCompany(DataRow row):this()
        {
            ID = row.Field<int>("ID");
            Company = row.Field<string>("NAME");
        }

        public ConfigCompany()
        {
            Selected = true;
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
