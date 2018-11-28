using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigAreaRepository
    {
        void DeleteConfigArea(ConfigArea c);
        ConfigArea GetConfigArea();
        ConfigArea ConfigAreaByID(int id);
        ObservableCollection<ConfigArea> GetConfigAreas();
        void UpdateConfigArea(ConfigArea configArea);
    }
}
