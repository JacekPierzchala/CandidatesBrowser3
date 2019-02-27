using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using CandidatesBrowser3.Extensions;

namespace CandidatesBrowser3.ViewModel
{
    public class AssignNewProjectViewModel : INotifyPropertyChanged
    {
        #region fields
        private IConfigStatusLibRepository configStatusLibRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectRepository configProjectRepository;
        #endregion


        private ObservableCollection<ConfigProjectsLib> configPojectsCollection;
        public ObservableCollection<ConfigProjectsLib> ConfigPojectsCollection
        {
            get { return configPojectsCollection; }
            set {
                configPojectsCollection = value;
                RaisePropertyChange("ConfigPojectsCollection");
                }
        }

        private ObservableCollection<ConfigStatusLib> configStatusLibCollection;
        public ObservableCollection<ConfigStatusLib> ConfigStatusLibCollection
        {
            get { return configStatusLibCollection; }
            set {
                configStatusLibCollection = value;
                RaisePropertyChange("ConfigStatusLibCollection");
                }
        }




        public AssignNewProjectViewModel(IConfigStatusLibRepository configStatusLibRepository,
            IConfigProjectsLibRepository configProjectsLibRepository, IConfigProjectRepository configProjectRepository)
        {
            this.configStatusLibRepository = configStatusLibRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectRepository = configProjectRepository;


            MessengerProject.Default.Register<ConfigProject>(this, OnProjectReceived);

        }

        private void OnProjectReceived(ConfigProject obj)
        {
            loadData();
        }

        private void loadData()
        {
            ConfigStatusLibCollection = configStatusLibRepository.GetConfigStatusLibs();
        }

        public void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
