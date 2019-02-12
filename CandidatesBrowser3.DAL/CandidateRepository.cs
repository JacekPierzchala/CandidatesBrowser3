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
    public class CandidateRepository:ICandidateRepository
    {
        private static ObservableCollection<Candidate> Candidates;
        private static Dictionary<dynamic, dynamic> Args= new Dictionary<dynamic, dynamic>();

        public CandidateRepository() {   }

        public Candidate CandidateByID(int id)
        {
           if(Candidates==null)
           {
                LoadCandidates();
           }
            return Candidates.Where(c => c.ID.Equals(id)).FirstOrDefault();
        }

        private void LoadCandidates()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM [Candidates_View]");
            Candidates = new ObservableCollection<Candidate>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Candidates.Add(new Candidate(row));
                    }
                }
            }
           
            catch (Exception x) {}
        }

        public void DeleteCandidate(Candidate candidate)
        {
            Candidates.Remove(candidate);
        }

        public Candidate GetCandidate()
        {
            if (Candidates == null)
            {
                LoadCandidates();
            }

            return Candidates.FirstOrDefault();
        }

        public ObservableCollection<Candidate> GetCandidates()
        {
            if (Candidates==null)
            {
                LoadCandidates();
            }
            return Candidates;
        }

        public void UpdateCandidate(Candidate candidate)
        {
            Candidate candidateToUpdate = Candidates.Where(c => c.ID.Equals(candidate.ID)).FirstOrDefault();
            candidateToUpdate = candidate;
        }

        public void UpdateCandidateDocumentInfo(Candidate candidate)
        {
            Args.Clear();
            Args.Add("@CandidateID", candidate.ID);
            Args.Add("@CVUploaded", candidate.CvUploaded);

            DBObjects.ExecProcedureWithArgs("UPDATE_DOCUMENT_INFO", Args);

        }
    }
}
