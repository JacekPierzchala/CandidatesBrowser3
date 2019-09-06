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
    public class CandidateCompanyRepository: ICandidateCompanyRepository
    {
        private static ObservableCollection<CandidateCompany> CandidateCompanys;
        private static Dictionary<dynamic, dynamic> Args = new Dictionary<dynamic, dynamic>();

        public CandidateCompanyRepository() { }

        public CandidateCompany CandidateCompanyByID(int CandidateID, int companyId)
        {
            if (CandidateCompanys == null)
            {
                LoadCandidateCompanys();
            }
            return CandidateCompanys.Where(c => c.CandidateID.Equals(CandidateID) && c.ID.Equals(companyId)).FirstOrDefault();
        }

        private void LoadCandidateCompanys()
        {
            DataTable dt = DBObjects.GetTableFromSQL(@"Select * FROM 
                                                    CONFIG_COMPANY CC INNER JOIN
                                                    [CANDIDATE_COMPANY] C ON CC.ID = C.COMPANY_ID");
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
            
                LoadCandidateCompanys();
            
            return CandidateCompanys;
        }

        public void UpdateCandidateCompany(CandidateCompany CandidateCompany)
        {
            CandidateCompany CandidateCompanyToUpdate = CandidateCompanys.Where(c => c.ID.Equals(CandidateCompany.ID)).FirstOrDefault();
            CandidateCompanyToUpdate = CandidateCompany;
        }

        public int AddCandidateCompany(int candidateId, ConfigCompany configCompany, string position, ConfigProject configProject)
        {
            Args.Clear();
            Args.Add("@CompanyID", configCompany.ID);
            Args.Add("@CandidateID", candidateId);
            Args.Add("@Position", position);
            Args.Add("@ProjectID", configProject.ID);

            try
            {
                return int.Parse(DBObjects.GetExecProcedureWithArgsResult("ADD_NEW_CANDIDATE_COMPANY", Args).ToString());
            }
            catch
            {
                return 0;
            }
        }

        public CandidateCompany AddAndCreateCandidateCompany(int candidateId,  string position)
        {
            CandidateCompany candidateCompany = new CandidateCompany();
            Args.Clear();
            Args.Add("@CandidateID", candidateId);
            Args.Add("@Position", position);
            

            int id = int.Parse(DBObjects.GetExecProcedureWithArgsResult("ADD_NEW_CANDIDATE_COMPANY", Args).ToString());

            candidateCompany.ID = id;
            candidateCompany.CandidateID = candidateId;
            candidateCompany.Position = position;
            candidateCompany.ProjectID = 0;
            return candidateCompany;
        }
    }
}
