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
    public class ConfigProjectRepository:IConfigProjectRepository
    {
        private static Dictionary<dynamic, dynamic> Args = new Dictionary<dynamic, dynamic>();
        private static ObservableCollection<ConfigProject> ConfigProjects;

        public void DeleteConfigProject(ConfigProject configProject)
        {
            ConfigProjects.Remove(configProject);
        }

        public ConfigProject GetConfigProject()
        {
            if (ConfigProjects== null)
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
            return ConfigProjects.Where(e => e.ID.Equals(id)).FirstOrDefault();
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
           
                LoadConfigProjects();
           
            return ConfigProjects;
        }

        public void UpdateConfigProject(ConfigProject configProject)
        {
            ConfigProject configProjectToUpdate = ConfigProjects.Where(e => e.ID.Equals(configProject.ID)).FirstOrDefault();
            configProjectToUpdate = configProject;
        }

        public void UpdateConfigProjectDocumentInfo(ConfigProject configProject)
        {
            Args.Clear();
            Args.Add("@ConfigProjectID", configProject.ID);
            Args.Add("@JDUploaded", configProject.JdUploaded);

            DBObjects.ExecSqlProcedure("UPDATE_PROJECT_DOCUMENT_INFO", Args);
        }

        public int AddNewConfigProjects(int ConfigProjectsLibId, int ConfigAreaId)
        {
            Args.Clear();
            Args.Add("@ConfigProjectsLibId", ConfigProjectsLibId);
            Args.Add("@ConfigAreaId", ConfigAreaId);

            return int.Parse(DBObjects.GetExecProcedureWithArgsResult("ADD_NEW_PROJECT_ID", Args).ToString());
        }

    }
}
