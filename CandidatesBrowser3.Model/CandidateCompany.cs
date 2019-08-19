using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class CandidateCompany : ConfigCompany, INotifyPropertyChanged, ICloneable
    {
        public int CandidateCompanyId { get; set; }
        private int candidateID;
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }

        private string position;
        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private int projectID;
        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        public CandidateCompany(DataRow row)
       {   
            CandidateID= row.Field<int>("CANDIDATE_ID");
            base.ID= row.Field<int>("COMPANY_ID");
            ProjectID= row.Field<int>("PROJECT_ID");
            Position= row.Field<string>("POSITION");
            base.Company = row.Field<string>("NAME");
            CandidateCompanyId = row.Field<int>("ID");

        }

        public CandidateCompany()
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


        public override string ToString()
        {
            return base.ID + " (" + base.Company + ")";
        }

        public new object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
