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
        private static IConfigProjectsRepository configProjectsRepository = new ConfigProjectsRepository();
        private static MainListViewModel mainListViewModel = new MainListViewModel(candidateRepository, configProjectsLibRepository, configProjectsRepository);

        public static MainListViewModel MainListViewModel
        {
            get
            {
                return mainListViewModel;
            }
        }
    }
}
