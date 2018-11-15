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
   public class ConfigProjectsLibRepository:IConfigProjectsLibRepository
    {
        private static ObservableCollection<ConfigProjectsLib> ConfigProjectsLibs;
        public void DeleteConfigProjectsLib(ConfigProjectsLib configProjectsLib)
        {
            ConfigProjectsLibs.Remove(configProjectsLib);
        }

        public ConfigProjectsLib GetConfigProjectsLib()
        {
            if (ConfigProjectsLibs==null)
            {
                LoadConfigProjectsLibs();
            }

            return ConfigProjectsLibs.FirstOrDefault();
        }
        public ConfigProjectsLib ConfigProjectsLibByID(int id)
        {
            if (ConfigProjectsLibs==null)
            {
                LoadConfigProjectsLibs();
            }
            return ConfigProjectsLibs.Where(e => e.Id.Equals(id)).FirstOrDefault();
        }
        private void LoadConfigProjectsLibs()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM [CONFIG_PROJECT_LIB] ORDER BY PROJECT_NAME");
            ConfigProjectsLibs = new ObservableCollection<ConfigProjectsLib>();
            try
            {
                if (dt!=null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigProjectsLibs.Add(new ConfigProjectsLib(row));
                    }
                }              
            }
            catch(Exception ex)
            { }
           
        }

        public ObservableCollection<ConfigProjectsLib> GetConfigProjectsLibs()
        {
            if (ConfigProjectsLibs==null)
            {
                LoadConfigProjectsLibs();
            }
            return ConfigProjectsLibs;
        }

        public void UpdateConfigProjectsLib(ConfigProjectsLib configProjectsLib)
        {
            ConfigProjectsLib configProjectsLibToUpdate = ConfigProjectsLibs.Where(e => e.Id.Equals(configProjectsLib.Id)).FirstOrDefault();
            configProjectsLibToUpdate = configProjectsLib;
        }
    }
}
