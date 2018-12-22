using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface ICandidateHistoryRepository
    {
        void DeleteCandidateHistory(CandidateHistory CandidateHistory);
        
        ObservableCollection<CandidateHistory> GetCandidateHistorysByID(int id);
        void UpdateCandidateHistory(CandidateHistory CandidateHistory);
    }
}
