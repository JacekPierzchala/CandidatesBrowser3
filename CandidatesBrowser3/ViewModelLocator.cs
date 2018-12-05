using CandidatesBrowser3.DAL;
using CandidatesBrowser3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3
{
    public class ViewModelLocator
    {
        private static ICandidateRepository candidateRepository = new CandidateRepository();
        private static IConfigProjectsLibRepository configProjectsLibRepository = new ConfigProjectsLibRepository();
        private static IConfigProjectsCandidateRepository configProjectsCandidateRepository = new ConfigProjectsCandidateRepository();
        private static IConfigAreaRepository configAreaRepository = new ConfigAreaRepository();
        private static IConfigProjectRepository configProjectRepository = new ConfigProjectRepository();
        private static IConfigCompanyRepository configCompanyRepository = new ConfigCompanyRepository();
        private static ICandidateCompanyRepository candidateCompanyRepository = new CandidateCompanyRepository();
        private static IConfigCompanyProjectRepository configCompanyProjectRepository = new ConfigCompanyProjectRepository();

        private static MainListViewModel mainListViewModel = 
            new MainListViewModel(candidateRepository, configProjectsLibRepository, 
            configProjectsCandidateRepository,configAreaRepository,configProjectRepository, 
            configCompanyRepository,candidateCompanyRepository,configCompanyProjectRepository);

        public static MainListViewModel MainListViewModel
        {
            get
            {
                return mainListViewModel;
            }
        }
    }
}
