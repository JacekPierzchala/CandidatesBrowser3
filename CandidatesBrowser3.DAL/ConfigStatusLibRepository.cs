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
    public class ConfigStatusLibRepository: IConfigStatusLibRepository
    {
        private static ObservableCollection<ConfigStatusLib> ConfigStatusLibs;
        public void DeleteConfigStatusLib(ConfigStatusLib ConfigStatusLib)
        {
            ConfigStatusLibs.Remove(ConfigStatusLib);
        }

        public ConfigStatusLib GetConfigStatusLib()
        {
            if (ConfigStatusLibs == null)
            {
                LoadConfigStatusLibs();
            }

            return ConfigStatusLibs.FirstOrDefault();
        }
        public ConfigStatusLib ConfigStatusLibByID(int id)
        {
            if (ConfigStatusLibs == null)
            {
                LoadConfigStatusLibs();
            }
            return ConfigStatusLibs.Where(e => e.ID.Equals(id)).FirstOrDefault();
        }
        private void LoadConfigStatusLibs()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM [CONFIG_STATUS_LIB] ORDER BY [DESCRIPTION]");
            ConfigStatusLibs = new ObservableCollection<ConfigStatusLib>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigStatusLibs.Add(new ConfigStatusLib(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<ConfigStatusLib> GetConfigStatusLibs()
        {
            if (ConfigStatusLibs == null)
            {
                LoadConfigStatusLibs();
            }
            return ConfigStatusLibs;
        }

        public void UpdateConfigStatusLib(ConfigStatusLib ConfigStatusLib)
        {
            ConfigStatusLib ConfigStatusLibToUpdate = ConfigStatusLibs.Where(e => e.ID.Equals(ConfigStatusLib.ID)).FirstOrDefault();
            ConfigStatusLibToUpdate = ConfigStatusLib;
        }
    }
}
