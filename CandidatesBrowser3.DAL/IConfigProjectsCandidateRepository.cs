using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigProjectsCandidateRepository
    {
        void DeleteConfigProjectCandidate(ConfigProjectCandidate configProject);
        ConfigProjectCandidate GetConfigProjectCandidate();
        ConfigProjectCandidate ConfigProjectCandidateByID(int id);
        ObservableCollection<ConfigProjectCandidate> GetConfigProjects();
        void UpdateConfigProjectCandidate(ConfigProjectCandidate configProject);
    }
}
