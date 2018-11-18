using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigProjectsRepository
    {
        void DeleteConfigProject(ConfigProject candidate);
        ConfigProject GetConfigProject();
        ConfigProject ConfigProjectByID(int id);
        ObservableCollection<ConfigProject> GetConfigProjects();
        void UpdateConfigProject(ConfigProject configProject);
    }
}
