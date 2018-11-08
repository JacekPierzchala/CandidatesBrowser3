using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Model
{
    public class Candidate : INotifyPropertyChanged, ICloneable
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

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                RaisePropertyChange("LastName");
            }
        }

        private string firstEmail;
        public string FirstEmail
        {
            get { return firstEmail; }
            set {
                firstEmail = value;
                RaisePropertyChange("FirstEmail");
            }
        }

        private string secondEmail;
        public string SecondEmail
        {
            get { return secondEmail; }
            set
            {
                secondEmail = value;
                RaisePropertyChange("SecondEmail");
            }
        }

        private string firstPhone;
        public string FirstPhone
        {
            get { return firstEmail; }
            set
            {
                firstPhone = value;
                RaisePropertyChange("FirstPhone");
            }
        }

        private string secondPhone;
        public string SecondPhone
        {
            get { return secondPhone; }
            set
            {
                secondPhone = value;
                RaisePropertyChange("SecondPhone");
            }
        }

        private string projects;
        public string Projects
        {
            get { return projects; }
            set {
                projects = value;
                RaisePropertyChange("Projects");
            }
        }

        private string area;
        public string Area
        {
            get { return area; }
            set
            {
                area = value;
                RaisePropertyChange("Area");
            }
        }

        private bool cvUploaded;
        public bool CvUploaded
        {
            get { return cvUploaded; }
            set
            {
                cvUploaded = value;
                RaisePropertyChange("CvUploaded");
            }
        }


        private bool deleted;
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value;
                RaisePropertyChange("Deleted");
                }   
        }


        public Candidate() { }
        public Candidate(DataRow row)
        {
            ID = row.Field<int>("ID");            
            FirstName = row.Field<string>("FIRST_NAME");            
            LastName = row.Field<string>("LAST_NAME");           
            FirstEmail = row.Field<string>("1ST_@");
            SecondEmail = row.Field<string>("2ND_@"); 
            FirstPhone = row.Field<string>("1ST_TEL");          
            SecondPhone = row.Field<string>("2ND_TEL"); 
            Area = row.Field<string>("AREA"); 
            Deleted = row.Field<bool>("DELETED");        
            Projects = row.Field<string>("PROJECTS");             
            CvUploaded = row.Field<bool>("CV_UPLOADED"); 
        }

        public void RaisePropertyChange(string propertyName)
        {
            if(PropertyChanged!=null)
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
