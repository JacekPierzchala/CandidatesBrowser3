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
using System.Threading;

namespace CandidatesBrowser3.ViewModel
{
    public class ProjectsReportViewModel: INotifyPropertyChanged
    {
        public static string[] Columns = 
            {
                "FirstName",
                "LastName",
                "Position",
                "CompanyName",
                "FirstEmail",
                "FirstPhone",
                "StatusName",
                "Comments"
            };

        private IConfigProjectRepository configProjectRepository;
        private ICandidateHistoryRepository candidateHistoryRepository;

        public ICommand ProjectSelectionChangeCommand { get; set; }
        public ICommand ExportToFileCommand { get; set; }

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
            CandidateHistoryCollection = candidateHistoryRepository.LoadHistorysByProjectID(SelectedProject.ID);
        }
        #endregion

        #region ExportToFileCommand 
        private bool CanExportToFile(object obj)
        {
            return (obj != null && ((ObservableCollection<CandidateHistory>)obj).Count > 0);
        
        }

        private void ExportToFile(object obj)
        {
            new Thread(() =>
            {
                //Thread.CurrentThread.IsBackground = true;

                GlobalFunctions.ExportToExcel(GlobalFunctions.ToDataTableFromList(CandidateHistoryCollection.ToList(),Columns));

            }).Start();
           
           
           // CandidateHistoryCollection = candidateHistoryRepository.LoadHistorysByProjectID(SelectedProject.ID);
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
            ExportToFileCommand = new CustomCommand(ExportToFile, CanExportToFile);
        }

      

        private void loadData()
        {
            ConfigProjectCollection = configProjectRepository.GetConfigProjects().OrderBy(e=>e.ProjectName).ToObservableCollection();
         
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
