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
    public class CandidateHistoryRepository: ICandidateHistoryRepository
    {
        private static ObservableCollection<CandidateHistory> CandidateHistorys;
        public void DeleteCandidateHistory(CandidateHistory CandidateHistory)
        {
            CandidateHistorys.Remove(CandidateHistory);
        }

        public CandidateHistory CandidateHistoryByID(int id)
        {
            if (CandidateHistorys == null)
            {
                LoadCandidateHistorysByID(id);
            }
            return CandidateHistorys.Where(e => e.ID.Equals(id)).FirstOrDefault();
        }
        private void LoadCandidateHistorysByID(int id)
        {
            Dictionary<string, string> Args = new Dictionary<string, string>();
            Args.Add("@CANDIDATE_ID", id.ToString());
            DataTable dt = DBObjects.GetTableFromSQL("FIND_CANDIDATE_HISTORY", Args);
            CandidateHistorys = new ObservableCollection<CandidateHistory>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CandidateHistorys.Add(new CandidateHistory(row));
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public ObservableCollection<CandidateHistory> GetCandidateHistorysByID(int id)
        {
           
                LoadCandidateHistorysByID(id);
            
            return CandidateHistorys;
        }

        public void UpdateCandidateHistory(CandidateHistory CandidateHistory)
        {
            CandidateHistory CandidateHistoryToUpdate = CandidateHistorys.Where(e => e.ID.Equals(CandidateHistory.ID)).FirstOrDefault();
            CandidateHistoryToUpdate = CandidateHistory;
        }
    }
}
