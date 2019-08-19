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
using CommonUlitlities;
namespace CandidatesBrowser3.ViewModel
{
    public class AssignNewProjectViewModel : INotifyPropertyChanged
    {
        #region fields
        private IConfigStatusLibRepository configStatusLibRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectRepository configProjectRepository;
        private IConfigCompanyRepository configCompanyRepository;
        private ICandidateCompanyRepository candidateCompanyRepository;
        private IConfigProjectsCandidateRepository configProjectsCandidateRepository;
        private ICandidateHistoryRepository candidateHistoryRepository;

        private IDialogService dialogService;
        #endregion

        #region commands
        public ICommand AssignNewProjectCommand { get; set; }
        #endregion

        #region methodsForcommands

        #region AssignNewProjectCommand
        private void AssignNewProject(object obj)
        {
            //int? newcandidateCompanyId = null;
            int? newCompanyId;
            int? newcandidateProjectsId;

            CandidateHistory = new CandidateHistory();

            if (ReceivedCandidateProjects.Where(e=>e.ConfigProjectID.Equals(SelectedProject.ID)).Any())
            {
                MessengerCandidateHistory.Default.Send<CandidateHistory>(CandidateHistory);
                return;
            }

            if (!ConfigCompanyCollectionAll.Where(e => e.Company.ToUpper().Equals(obj.ToString().ToUpper())).Any())
            {
                newCompanyId = configCompanyRepository.AddConfigCompany(obj.ToString());
                ConfigCompanyCollectionAll.Add(new ConfigCompany() { ID = (int)newCompanyId, Company = obj.ToString() });

            }
            

            newcandidateProjectsId = configProjectsCandidateRepository.AddConfigProjectCandidate
                                    (ReceivedCandidateProjects.Select(e=>e.ConfigCandidateID).FirstOrDefault(), SelectedProject, SelectedPosition,
                                     ConfigCompanyCollectionAll.Where(e => e.Company.ToUpper().Equals(obj.ToString().ToUpper())).FirstOrDefault().ID);
            


                CandidateHistory.CandidateID = ReceivedCandidateProjects.Select(e => e.ConfigCandidateID).FirstOrDefault(); ;
                CandidateHistory.ProjectID = SelectedProject.ID;
                CandidateHistory.ProjectName =SelectedProject.ProjectName;
                CandidateHistory.Position =SelectedPosition;
                CandidateHistory.Timestamp =DateTime.Now;
                CandidateHistory.StatusName =SelectedStatus.Description;
                CandidateHistory.CompanyID = ConfigCompanyCollectionAll.Where(e => e.Company.ToUpper().Equals(obj.ToString().ToUpper())).FirstOrDefault().ID;
                CandidateHistory.CompanyName = ConfigCompanyCollectionAll.Where(e => e.Company.ToUpper().Equals(obj.ToString().ToUpper())).FirstOrDefault().Company;
                CandidateHistory.ConfigAreaId =SelectedProject.ConfigAreaID;
                CandidateHistory.AreaName =SelectedProject.AreaName;
                CandidateHistory.Seq = 1;
                CandidateHistory.ConfigStatusID = SelectedStatus.ID;
                CandidateHistory.Comments = Comment;
                CandidateHistory.CandidatesProjectsID = (int)newcandidateProjectsId;
                CandidateHistory.ConfigProjectLib = SelectedProject.ConfigProjectLibID;
               
            

            MessengerCandidateHistory.Default.Send<CandidateHistory>(CandidateHistory);



            //new CandidateCompany() { }
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

        #region properties
        private ObservableCollection<ConfigProjectCandidate> receivedCandidateProjects;
        public ObservableCollection<ConfigProjectCandidate> ReceivedCandidateProjects
        {
            get { return receivedCandidateProjects; }
            set
            {
                receivedCandidateProjects = value;
                RaisePropertyChange("ReceivedCandidateProjects");
            }
        }

        
        public CandidateHistory CandidateHistory { get; set; }
        


        private ObservableCollection<ConfigProject> configPojectsCollection;
        public ObservableCollection<ConfigProject> ConfigPojectsCollection
        {
            get { return configPojectsCollection; }
            set
            {
                configPojectsCollection = value;
                RaisePropertyChange("ConfigPojectsCollection");
            }
        }

        private ObservableCollection<ConfigStatusLib> configStatusLibCollection;
        public ObservableCollection<ConfigStatusLib> ConfigStatusLibCollection
        {
            get { return configStatusLibCollection; }
            set
            {
                configStatusLibCollection = value;
                RaisePropertyChange("ConfigStatusLibCollection");
            }
        }

        private ObservableCollection<ConfigCompany> configCompanyCollection;
        public ObservableCollection<ConfigCompany> ConfigCompanyCollection
        {
            get { return configCompanyCollection; }
            set
            {
                configCompanyCollection = value;
                RaisePropertyChange("ConfigCompanyCollection");
            }
        }

        private ObservableCollection<ConfigCompany> configCompanyCollectionAll;
        public ObservableCollection<ConfigCompany> ConfigCompanyCollectionAll
        {
            get { return configCompanyCollectionAll; }
            set
            {
                configCompanyCollectionAll = value;
                RaisePropertyChange("ConfigCompanyCollectionAll");
            }
        }

        private string selectedPosition;
        public string SelectedPosition
        {
            get { return selectedPosition; }
            set
            {
                selectedPosition = value;
                RaisePropertyChange("SelectedPosition");
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set {
                comment = value;
                RaisePropertyChange("Comment");
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

        private ConfigStatusLib selectedStatus;
        public ConfigStatusLib SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                RaisePropertyChange("SelectedStatus");
            }
        }

        private ConfigCompany selectedCompany;
        public ConfigCompany SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                selectedCompany = value;
                RaisePropertyChange("SelectedCompany");
            }
        }

        #endregion



        public AssignNewProjectViewModel(IConfigStatusLibRepository configStatusLibRepository,
            IConfigProjectsLibRepository configProjectsLibRepository, IConfigProjectRepository configProjectRepository, IConfigCompanyRepository configCompanyRepository,
            IDialogService dialogService, ICandidateCompanyRepository candidateCompanyRepository, 
            IConfigProjectsCandidateRepository configProjectsCandidateRepository, ICandidateHistoryRepository candidateHistoryRepository)
        {
            this.dialogService = dialogService;
            this.configStatusLibRepository = configStatusLibRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectRepository = configProjectRepository;
            this.configCompanyRepository = configCompanyRepository;
            this.candidateCompanyRepository = candidateCompanyRepository;
            this.configProjectsCandidateRepository = configProjectsCandidateRepository;
            this.candidateHistoryRepository = candidateHistoryRepository;

           
            MessengerCandidateProject.Default.Register<ObservableCollection<ConfigProjectCandidate>>(this, OnCandidateCompanyReceived);
            MessengerCandidateHistory.Default.Register<UpdateCandidateHistory>(this, OnUpdateListMessageReceived);

        }

        private void OnUpdateListMessageReceived(UpdateCandidateHistory obj)
        {
           
        }

        private void OnCandidateCompanyReceived(ObservableCollection<ConfigProjectCandidate> args)
        {
            ReceivedCandidateProjects = args;
            loadData();
            loadCommands();
        }
        private void loadCommands()
        {
            AssignNewProjectCommand = new CustomCommand(AssignNewProject, CanAssignNewProject);
        }

        private void loadData()
        {
            SelectedPosition = "";
            Comment = "";
            SelectedCompany = null;
            SelectedProject = null;
            SelectedStatus = null;
            ConfigStatusLibCollection = configStatusLibRepository.GetConfigStatusLibs();
            ConfigPojectsCollection = configProjectRepository.GetConfigProjects().OrderBy(e=>e.ProjectName).ToObservableCollection();
            ConfigCompanyCollection = configCompanyRepository.GetConfigCompanysForCandidate(ReceivedCandidateProjects).OrderBy(e => e.Company).ToObservableCollection() ;
            ConfigCompanyCollectionAll = configCompanyRepository.GetConfigCompanys().ToObservableCollection();
           
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
