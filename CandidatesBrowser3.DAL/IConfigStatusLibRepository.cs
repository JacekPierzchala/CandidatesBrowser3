using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigStatusLibRepository
    {
        void DeleteConfigStatusLib(ConfigStatusLib ConfigStatusLib);
        ConfigStatusLib GetConfigStatusLib();
        ConfigStatusLib ConfigStatusLibByID(int id);
        ObservableCollection<ConfigStatusLib> GetConfigStatusLibs();
        void UpdateConfigStatusLib(ConfigStatusLib ConfigStatusLib);
    }
}
