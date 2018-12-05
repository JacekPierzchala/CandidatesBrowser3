using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface ICandidateCompanyRepository
    {
        void DeleteCandidateCompany(CandidateCompany CandidateCompany);
        CandidateCompany GetCandidateCompany();
        CandidateCompany CandidateCompanyByID(int id);
        ObservableCollection<CandidateCompany> GetCandidateCompanys();
        void UpdateCandidateCompany(CandidateCompany CandidateCompany);
    }
}
