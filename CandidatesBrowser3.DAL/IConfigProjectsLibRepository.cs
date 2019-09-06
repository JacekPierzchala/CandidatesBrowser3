using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandidatesBrowser3.Model;
using System.Collections.ObjectModel;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigProjectsLibRepository
    {
        void DeleteConfigProjectsLib(ConfigProjectsLib configProjectsLib);
        ConfigProjectsLib GetConfigProjectsLib();
        ConfigProjectsLib ConfigProjectsLibByID(int id);
        ObservableCollection<ConfigProjectsLib> GetConfigProjectsLibs();
        void UpdateConfigProjectsLib(ConfigProjectsLib configProjectsLib);

        int AddNewConfigProjectsLib(string ProjectName);
    }
}
