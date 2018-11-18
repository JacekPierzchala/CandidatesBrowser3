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
    public class ConfigProjectsRepository: IConfigProjectsRepository
    {

        private static ObservableCollection<ConfigProject> ConfigProjects;
        public void DeleteConfigProject(ConfigProject configProject)
        {
            ConfigProjects.Remove(configProject);
        }

        public ConfigProject GetConfigProject()
        {
            if (ConfigProjects == null)
            {
                LoadConfigProjects();
            }

            return ConfigProjects.FirstOrDefault();
        }
        public ConfigProject ConfigProjectByID(int id)
        {
            if (ConfigProjects == null)
            {
                LoadConfigProjects();
            }
            return ConfigProjects.Where(e => e.ConfigProjectID.Equals(id)).FirstOrDefault();
        }
        private void LoadConfigProjects()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM ConfigProject_View ");
            ConfigProjects = new ObservableCollection<ConfigProject>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigProjects.Add(new ConfigProject(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<ConfigProject> GetConfigProjects()
        {
            if (ConfigProjects == null)
            {
                LoadConfigProjects();
            }
            return ConfigProjects;
        }

        public void UpdateConfigProject(ConfigProject configProject)
        {
            ConfigProject configProjectsToUpdate = ConfigProjects.Where(e => e.ConfigProjectID.Equals(configProject.ConfigProjectID)).FirstOrDefault();
            configProjectsToUpdate = configProject;
        }
    }
}
