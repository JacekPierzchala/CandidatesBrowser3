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
   public class ConfigCompanyProjectRepository: IConfigCompanyProjectRepository
   {
        private static ObservableCollection<ConfigCompanyProject> ConfigCompanyProjects;

        public ConfigCompanyProjectRepository() { }

        public ConfigCompanyProject ConfigCompanyProjectByID(int id)
        {
            if (ConfigCompanyProjects == null)
            {
                LoadConfigCompanyProjects();
            }
            return ConfigCompanyProjects.Where(c => c.CompanyID.Equals(id)).FirstOrDefault();
        }

        private void LoadConfigCompanyProjects()
        {
            DataTable dt = DBObjects.GetTableFromSQL("SELECT DISTINCT [COMPANY_ID],[PROJECT_ID] FROM[CANDIDATE_COMPANY]");
            ConfigCompanyProjects = new ObservableCollection<ConfigCompanyProject>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigCompanyProjects.Add(new ConfigCompanyProject(row));
                    }
                }
            }

            catch (Exception x) { }
        }

        public void DeleteConfigCompanyProject(ConfigCompanyProject ConfigCompanyProject)
        {
            ConfigCompanyProjects.Remove(ConfigCompanyProject);
        }

        public ConfigCompanyProject GetConfigCompanyProject()
        {
            if (ConfigCompanyProjects == null)
            {
                LoadConfigCompanyProjects();
            }

            return ConfigCompanyProjects.FirstOrDefault();
        }

        public ObservableCollection<ConfigCompanyProject> GetConfigCompanyProjects()
        {
            if (ConfigCompanyProjects == null)
            {
                LoadConfigCompanyProjects();
            }
            return ConfigCompanyProjects;
        }

        public void UpdateConfigCompanyProject(ConfigCompanyProject ConfigCompanyProject)
        {
            ConfigCompanyProject ConfigCompanyProjectToUpdate = ConfigCompanyProjects.Where(c => c.CompanyID.Equals(ConfigCompanyProject.CompanyID)).FirstOrDefault();
            ConfigCompanyProjectToUpdate = ConfigCompanyProject;
        }
    }
}

