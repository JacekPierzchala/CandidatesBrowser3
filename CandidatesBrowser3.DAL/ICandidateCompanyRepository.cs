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
        CandidateCompany CandidateCompanyByID(int CandidateID, int companyId);
        ObservableCollection<CandidateCompany> GetCandidateCompanys();
        int AddCandidateCompany(int candidateID, ConfigCompany configCompany, string position, ConfigProject configProject);
        void UpdateCandidateCompany(CandidateCompany CandidateCompany);
        CandidateCompany AddAndCreateCandidateCompany(int candidateID, string position);
    }
}
