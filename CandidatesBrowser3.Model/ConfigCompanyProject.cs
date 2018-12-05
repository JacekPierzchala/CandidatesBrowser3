using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class ConfigCompanyProject : ConfigCompany, INotifyPropertyChanged, ICloneable
    {
        private int projectID;
        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        private int id;

        public new  int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int companyID;
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }



        public ConfigCompanyProject()
        {

        }

        public ConfigCompanyProject(DataRow row)
        {
            ProjectID = row.Field<int>("PROJECT_ID");
            CompanyID = row.Field<int>("COMPANY_ID");
            
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
