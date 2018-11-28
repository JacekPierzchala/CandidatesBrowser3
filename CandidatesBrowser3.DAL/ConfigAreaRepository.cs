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
    public class ConfigAreaRepository:IConfigAreaRepository
    {

        private static ObservableCollection<ConfigArea> ConfigAreas;
        public void DeleteConfigArea(ConfigArea configArea)
        {
            ConfigAreas.Remove(configArea);
        }

        public ConfigArea GetConfigArea()
        {
            if (ConfigAreas == null)
            {
                LoadConfigAreas();
            }

            return ConfigAreas.FirstOrDefault();
        }
        public ConfigArea ConfigAreaByID(int id)
        {
            if (ConfigAreas == null)
            {
                LoadConfigAreas();
            }
            return ConfigAreas.Where(e => e.ID.Equals(id)).FirstOrDefault();
        }
        private void LoadConfigAreas()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM CONFIG_AREA ORDER BY AREA_NAME");
            ConfigAreas = new ObservableCollection<ConfigArea>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigAreas.Add(new ConfigArea(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<ConfigArea> GetConfigAreas()
        {
            if (ConfigAreas == null)
            {
                LoadConfigAreas();
            }
            return ConfigAreas;
        }

        public void UpdateConfigArea(ConfigArea configArea)
        {
            ConfigArea configAreasToUpdate = ConfigAreas.Where(e => e.ID.Equals(configArea.ID)).FirstOrDefault();
            configAreasToUpdate = configArea;
        }
    }
}
