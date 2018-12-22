using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class CandidateHistory :Candidate, INotifyPropertyChanged, ICloneable
    {
        private int id;
        public new int ID
        {
            get { return id; }
            set {
                id = value;
                RaisePropertyChange("ID");
                }
        }


        private int candidateID;
        public int CandidateID
        {
            get { return candidateID; }
            set {
                candidateID = value;
                RaisePropertyChange("CandidateID");
                }
        }

        private int companyID;
        public int CompanyID
        {
            get { return companyID; }
            set {
                companyID = value;
                RaisePropertyChange("CompanyID");
                }
        }

        private string position;
        public string Position
        {
            get { return position; }
            set {
                position = value;
                RaisePropertyChange("Position");
                }
        }

        private int projectID;
        public int ProjectID
        {
            get { return projectID; }
            set {
                projectID = value;
                RaisePropertyChange("ProjectID");
                }
        }

        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
            set {
                timestamp = value;
                RaisePropertyChange("Timestamp");
                }
        }

        private int seq;
        public int Seq
        {
            get { return seq; }
            set {
                seq = value;
                RaisePropertyChange("Seq");
                }
        }

        private int configStatusID;
        public int ConfigStatusID
        {
            get { return configStatusID; }
            set {
                configStatusID = value;
                RaisePropertyChange("ConfigStatusID");
                }
        }

        private string  comments;
        public string Comments
        {
            get { return comments; }
            set {
                comments = value;
                RaisePropertyChange("Comments");
                }
        }

        private string historyOfContact;
        public string HistoryOfContact
        {
            get { return historyOfContact; }
            set {
                historyOfContact = value;
                RaisePropertyChange("HistoryOfContact");
                }
        }

        private string statusName;
        public string StatusName
        {
            get { return statusName; }
            set {
                statusName = value;
                RaisePropertyChange("StatusName");
                }
        }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                RaisePropertyChange("ProjectName");
            }
        }

        private string areaName;
        public string AreaName
        {
            get { return areaName; }
            set
            {
                areaName = value;
                RaisePropertyChange("AreaName");
            }
        }

        private string  companyName;
        public string CompanyName
        {
            get { return companyName; }
            set {
                companyName = value;
                RaisePropertyChange("CompanyName");
                }
        }

        public CandidateHistory()
        {

        }

        public CandidateHistory(DataRow row)
        {
            CandidateID = row.Field<int>("CANDIDATE_ID");
            CompanyID= row.Field<int>("COMPANY_ID");
            Position= row.Field<string>("POSITION");
            ProjectID= row.Field<int>("PROJECT_ID");
            Timestamp= row.Field<DateTime>("TIMESTAMP");
            Seq= row.Field<int>("SEQ");
            ConfigStatusID= row.Field<int>("CONFIG_STATUS_ID");
            ProjectName = row.Field<string>("PROJECT_NAME");
            Comments= row.Field<string>("COMMENTS");
            HistoryOfContact= row.Field<string>("HISTORY_OF_CONTACT");
            StatusName = row.Field<string>("STATUS_NAME");
            CompanyName = row.Field<string>("COMPANY_NAME");
            AreaName= row.Field<string>("AREA_NAME");

        }

        public new void RaisePropertyChange(string propertyName)
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

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
