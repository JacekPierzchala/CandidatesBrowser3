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
    public class CandidateCompanyRepository: ICandidateCompanyRepository
    {
        private static ObservableCollection<CandidateCompany> CandidateCompanys;

        public CandidateCompanyRepository() { }

        public CandidateCompany CandidateCompanyByID(int id)
        {
            if (CandidateCompanys == null)
            {
                LoadCandidateCompanys();
            }
            return CandidateCompanys.Where(c => c.ID.Equals(id)).FirstOrDefault();
        }

        private void LoadCandidateCompanys()
        {
            DataTable dt = DBObjects.GetTableFromSQL("Select * FROM [CANDIDATE_COMPANY]");
            CandidateCompanys = new ObservableCollection<CandidateCompany>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CandidateCompanys.Add(new CandidateCompany(row));
                    }
                }
            }

            catch (Exception x) { }
        }

        public void DeleteCandidateCompany(CandidateCompany CandidateCompany)
        {
            CandidateCompanys.Remove(CandidateCompany);
        }

        public CandidateCompany GetCandidateCompany()
        {
            if (CandidateCompanys == null)
            {
                LoadCandidateCompanys();
            }

            return CandidateCompanys.FirstOrDefault();
        }

        public ObservableCollection<CandidateCompany> GetCandidateCompanys()
        {
            if (CandidateCompanys == null)
            {
                LoadCandidateCompanys();
            }
            return CandidateCompanys;
        }

        public void UpdateCandidateCompany(CandidateCompany CandidateCompany)
        {
            CandidateCompany CandidateCompanyToUpdate = CandidateCompanys.Where(c => c.ID.Equals(CandidateCompany.ID)).FirstOrDefault();
            CandidateCompanyToUpdate = CandidateCompany;
        }

    }
}