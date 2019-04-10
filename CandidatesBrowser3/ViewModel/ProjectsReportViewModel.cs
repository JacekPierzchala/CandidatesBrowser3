using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using CommonUlitlities;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows;
using Prism.Interactivity.InteractionRequest;

namespace CandidatesBrowser3.ViewModel
{
    public class ProjectsReportViewModel: INotifyPropertyChanged
    {
        private IConfigProjectRepository configProjectRepository;
        private ICandidateHistoryRepository candidateHistoryRepository;

        public ICommand ProjectSelectionChangeCommand { get; set; }

        private ObservableCollection<CandidateHistory> candidateHistoryCollection;
        public ObservableCollection<CandidateHistory> CandidateHistoryCollection
        {
            get { return candidateHistoryCollection; }
            set {
                candidateHistoryCollection = value;
                RaisePropertyChange("CandidateHistoryCollection");
                }
        }


        private ObservableCollection<ConfigProject> configProjectCollection;
        public ObservableCollection<ConfigProject> ConfigProjectCollection
        {
            get { return configProjectCollection; }
            set
            {
                configProjectCollection = value;
                RaisePropertyChange("ConfigProjectCollection");
            }
        }

        private ConfigProject selectedProject;
        public ConfigProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                RaisePropertyChange("SelectedProject");
            }
        }

        #region ProjectSelectionChangeCommand
        private bool CanProjectSelectionChange(object obj)
        {
            if (SelectedProject !=null)
            {
                return true;
            }
            return false;
        }

        private void ProjectSelectionChange(object obj)
        {
            
        }
        #endregion


        public ProjectsReportViewModel(IConfigProjectRepository configProjectRepository, ICandidateHistoryRepository candidateHistoryRepository)
        {
            this.configProjectRepository = configProjectRepository;
            this.candidateHistoryRepository = candidateHistoryRepository;

            loadData();
            commandsInitialize();
        }

        private void commandsInitialize()
        {
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
        }

      

        private void loadData()
        {
            ConfigProjectCollection = configProjectRepository.GetConfigProjects();
            ConfigProjectCollection = ConfigProjectCollection.OrderBy(e => e.ProjectName).ToObservableCollection();
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
