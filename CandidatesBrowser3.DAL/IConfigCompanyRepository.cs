using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigCompanyRepository
    {
        void DeleteConfigCompany(ConfigCompany c);
        ConfigCompany GetConfigCompany();
        ConfigCompany ConfigCompanyByID(int id);
        ObservableCollection<ConfigCompany> GetConfigCompanys();
        void UpdateConfigCompany(ConfigCompany ConfigCompany);
        ObservableCollection<ConfigCompany> GetConfigCompanysForCandidate(CandidateCompany cc);
        int AddConfigCompany(ConfigCompany cc);
        int AddConfigCompany(string cc);
    }
}
