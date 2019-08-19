using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public class ConfigProjectsCandidateRepository : IConfigProjectsCandidateRepository
    {
        private static Dictionary<dynamic, dynamic> Args = new Dictionary<dynamic, dynamic>();
        private static ObservableCollection<ConfigProjectCandidate> ConfigProjectsCandidates;
        public void DeleteConfigProjectCandidate(ConfigProjectCandidate configProjectCandidate)
        {
            ConfigProjectsCandidates.Remove(configProjectCandidate);
        }

        public ConfigProjectCandidate GetConfigProjectCandidate()
        {
            if (ConfigProjectsCandidates == null)
            {
                LoadConfigProjectsCandidate();
            }

            return ConfigProjectsCandidates.FirstOrDefault();
        }
        public ConfigProjectCandidate ConfigProjectCandidateByID(int projectId, int candidateId)
        {
            if (ConfigProjectsCandidates == null)
            {
                LoadConfigProjectsCandidate();
            }
            return ConfigProjectsCandidates.Where(e => e.ConfigProjectID.Equals(projectId) && e.ConfigCandidateID.Equals(candidateId)).FirstOrDefault();
        }
        private void LoadConfigProjectsCandidate()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM ConfigProject_Candidates_View ");
            ConfigProjectsCandidates = new ObservableCollection<ConfigProjectCandidate>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigProjectsCandidates.Add(new ConfigProjectCandidate(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<ConfigProjectCandidate> GetConfigProjects()
        {
            if (ConfigProjectsCandidates == null)
            {
                LoadConfigProjectsCandidate();
            }
            return ConfigProjectsCandidates;
        }

        public void UpdateConfigProjectCandidate(ConfigProjectCandidate configProjectCandidate)
        {
            ConfigProjectCandidate configProjectsCandidateToUpdate = ConfigProjectsCandidates.Where(e => e.ConfigProjectID.Equals(configProjectCandidate.ConfigProjectID)).FirstOrDefault();
            configProjectsCandidateToUpdate = configProjectCandidate;
        }

        public int AddConfigProjectCandidate(int candidateId, ConfigProject configProject, string position, int companyId)
        {
            Args.Clear();          
            Args.Add("@CandidateID", candidateId);
            Args.Add("@ProjectID", configProject.ID);
            Args.Add("@Position", position);
            Args.Add("@CompanyID", companyId);


            
            try
            {
                return int.Parse(DBObjects.GetExecProcedureWithArgsResult("ADD_NEW_CANDIDATE_PROJECT", Args).ToString());
            }
            catch (Exception)
            {
                return 0;
            }
           
        }
    }
}
