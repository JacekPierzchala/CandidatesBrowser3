using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandidatesBrowser3.Model;
using System.Collections.ObjectModel;

namespace CandidatesBrowser3.DAL
{
    public interface ICandidateRepository
    {
        void DeleteCandidate(Candidate candidate);
        Candidate GetCandidate();
        Candidate CandidateByID(int id);
        ObservableCollection<Candidate> GetCandidates();
        void UpdateCandidate(Candidate candidate);
        int AddCandidtate(Candidate candidate);
        void UpdateCandidateDocumentInfo(Candidate candidate);
    }
}
