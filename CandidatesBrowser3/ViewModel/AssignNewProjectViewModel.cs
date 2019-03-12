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
using System.Windows.Input;

namespace CandidatesBrowser3.ViewModel
{
    public class AssignNewProjectViewModel : INotifyPropertyChanged
    {
        #region fields
        private IConfigStatusLibRepository configStatusLibRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectRepository configProjectRepository;
        private IConfigCompanyRepository configCompanyRepository;
        private IDialogService dialogService;
        #endregion

        #region commands
        public ICommand AssignNewProjectCommand { get; set; }
        #endregion

        #region methodsForcommands

        #region AssignNewProjectCommand
        private void AssignNewProject(object obj)
        {

        }

        private bool CanAssignNewProject(object obj)
        {
            if(  !string.IsNullOrWhiteSpace(obj.ToString()) &&   !string.IsNullOrWhiteSpace(SelectedPosition) && SelectedProject !=null && SelectedStatus!=null )
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion

        private CandidateCompany receivedCandidateCompany;
        public CandidateCompany ReceivedCandidateCompany
        {
            get { return receivedCandidateCompany; }
            set {
                receivedCandidateCompany = value;
                RaisePropertyChange("ReceivedCandidateCompany");
                }
        }


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

        private ObservableCollection<ConfigCompany> configCompanyCollection;
        public ObservableCollection<ConfigCompany> ConfigCompanyCollection
        {
            get { return configCompanyCollection; }
            set {
                configCompanyCollection = value;
                RaisePropertyChange("ConfigCompanyCollection");
                }
        }

        private string selectedPosition;
        public string SelectedPosition
        {
            get { return selectedPosition; }
            set {
                selectedPosition = value;
                RaisePropertyChange("SelectedPosition");
                }
        }


        private ConfigProjectsLib selectedProject;
        public ConfigProjectsLib SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                RaisePropertyChange("SelectedProject");
            }
        }

        private ConfigStatusLib selectedStatus;
        public ConfigStatusLib SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                RaisePropertyChange("SelectedProject");
            }
        }

        private ConfigCompany selectedCompany;
        public ConfigCompany SelectedCompany
        {
            get { return selectedCompany; }
            set {
                selectedCompany = value;
                RaisePropertyChange("SelectedCompany");
                }
        }


        public AssignNewProjectViewModel(IConfigStatusLibRepository configStatusLibRepository,
            IConfigProjectsLibRepository configProjectsLibRepository, IConfigProjectRepository configProjectRepository, IConfigCompanyRepository configCompanyRepository,
            IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.configStatusLibRepository = configStatusLibRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectRepository = configProjectRepository;
            this.configCompanyRepository = configCompanyRepository;

            MessengerCandidateCompany.Default.Register<CandidateCompany>(this, OnCandidateCompanyReceived);

        }

        private void OnCandidateCompanyReceived(CandidateCompany obj)
        {
            ReceivedCandidateCompany = obj;
            loadData();
            loadCommands();
        }
        private void loadCommands()
        {
            AssignNewProjectCommand = new CustomCommand(AssignNewProject, CanAssignNewProject);
        }

        private void loadData()
        {
            ConfigStatusLibCollection = configStatusLibRepository.GetConfigStatusLibs();
            ConfigPojectsCollection = configProjectsLibRepository.GetConfigProjectsLibs();
            ConfigCompanyCollection = configCompanyRepository.GetConfigCompanysForCandidate(ReceivedCandidateCompany);
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
