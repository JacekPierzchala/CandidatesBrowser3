using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUlitlities;


namespace CandidatesBrowser3.DAL
{
    public class ConfigCompanyRepository: IConfigCompanyRepository
    {
        private static ObservableCollection<ConfigCompany> ConfigCompanys;
        public void DeleteConfigCompany(ConfigCompany ConfigCompany)
        {
            ConfigCompanys.Remove(ConfigCompany);
        }

        public ConfigCompany GetConfigCompany()
        {
            if (ConfigCompanys == null)
            {
                LoadConfigCompanys();
            }

            return ConfigCompanys.FirstOrDefault();
        }
        public ConfigCompany ConfigCompanyByID(int id)
        {
            if (ConfigCompanys == null)
            {
                LoadConfigCompanys();
            }
            return ConfigCompanys.Where(e => e.ID.Equals(id)).FirstOrDefault();
        }
        private void LoadConfigCompanys()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM [CONFIG_COMPANY] ORDER BY NAME ");
            ConfigCompanys = new ObservableCollection<ConfigCompany>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ConfigCompanys.Add(new ConfigCompany(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<ConfigCompany> GetConfigCompanys()
        {
            if (ConfigCompanys == null)
            {
                LoadConfigCompanys();
            }
            return ConfigCompanys;
        }

        public void UpdateConfigCompany(ConfigCompany ConfigCompany)
        {
            ConfigCompany ConfigCompanysToUpdate = ConfigCompanys.Where(e => e.ID.Equals(ConfigCompany.ID)).FirstOrDefault();
            ConfigCompanysToUpdate = ConfigCompany;
        }

        public ObservableCollection<ConfigCompany> GetConfigCompanysForCandidate(CandidateCompany cc)
        {
            return ConfigCompanys.Where(e => e.ID.Equals(cc.ID)).ToList().ToObservableCollection();
        }
    }
}
