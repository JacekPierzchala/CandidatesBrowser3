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
        public ConfigProjectCandidate ConfigProjectCandidateByID(int id)
        {
            if (ConfigProjectsCandidates == null)
            {
                LoadConfigProjectsCandidate();
            }
            return ConfigProjectsCandidates.Where(e => e.ConfigProjectID.Equals(id)).FirstOrDefault();
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
    }
}
