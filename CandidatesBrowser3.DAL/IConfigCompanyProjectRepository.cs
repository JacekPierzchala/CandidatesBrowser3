using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigCompanyProjectRepository
    {
        void DeleteConfigCompanyProject(ConfigCompanyProject ConfigCompanyProject);
        ConfigCompanyProject GetConfigCompanyProject();
        ConfigCompanyProject ConfigCompanyProjectByID(int id);
        ObservableCollection<ConfigCompanyProject> GetConfigCompanyProjects();
        void UpdateConfigCompanyProject(ConfigCompanyProject ConfigCompanyProject);
    }
}
