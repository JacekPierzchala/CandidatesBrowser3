﻿using CandidatesBrowser3.Model;
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
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM CANDIDATES");
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
            
                LoadCandidates();
           
            return Candidates;
        }

        public void UpdateCandidate(Candidate candidate)
        {
            //Candidate candidateToUpdate = Candidates.Where(c => c.ID.Equals(candidate.ID)).FirstOrDefault();
            //candidateToUpdate = candidate;
            Args.Clear();
            Args.Add("@CandidateId", candidate.ID);
            Args.Add("@FirstName", candidate.FirstName);
            Args.Add("@LastName", candidate.LastName);
            Args.Add("@FirstPhone", candidate.FirstPhone);
            Args.Add("@SecondPhone", candidate.SecondPhone);
            Args.Add("@FirstEmail", candidate.FirstEmail);
            Args.Add("@SecondEmail", candidate.SecondEmail);

            DBObjects.ExecSqlProcedure("UPDATE_CANDIDATE_INFO", Args);

        }

        public int AddCandidtate(Candidate candidate)
        {
            Args.Clear();
            Args.Add("@FirstEmail", candidate.FirstEmail);
            Args.Add("@SecondEmail", candidate.SecondEmail);
            Args.Add("@FirstName", candidate.FirstName);
            Args.Add("@LastName", candidate.LastName);
            Args.Add("@FirstPhone", candidate.FirstPhone);
            Args.Add("@SecondPhone", candidate.SecondPhone);

            return int.Parse(DBObjects.GetExecProcedureWithArgsResult("ADD_NEW_CANDIDATE", Args).ToString());
        }

        public void UpdateCandidateDocumentInfo(Candidate candidate)
        {
            Args.Clear();
            Args.Add("@CandidateID", candidate.ID);
            Args.Add("@CVUploaded", candidate.CvUploaded);

            DBObjects.ExecSqlProcedure("UPDATE_DOCUMENT_INFO", Args);

        }
    }
}
