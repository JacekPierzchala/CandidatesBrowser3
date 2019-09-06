using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandidatesBrowser3.Utilities;
using CandidatesBrowser3.DAL;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using CommonUlitlities;
using System.Data;
using System.IO;

namespace CandidatesBrowser3.ViewModel
{
    public class MainListViewModel : INotifyPropertyChanged
    {
        #region properties
        private ICandidateRepository candidateRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectsCandidateRepository configProjectsCandidateRepository;
        private IConfigAreaRepository configAreaRepository;
        private IConfigProjectRepository configProjectRepository;
        private IConfigCompanyRepository configCompanyRepository;
        private ICandidateCompanyRepository candidateCompanyRepository;
        private IConfigCompanyProjectRepository configCompanyProjectRepository;
        private IDialogService dialogService;
        private ConfigProjectMessenger configProjectMessenger;
        private bool combinedRefreshNeeded;
        public bool CombinedRefreshNeeded
        {
            get { return combinedRefreshNeeded; }
            set
            {
                combinedRefreshNeeded = value;
                RaisePropertyChange("CombinedRefreshNeeded");
            }
        }

        private bool? cvUploaded;
        public bool? CvUploaded
        {
            get { return cvUploaded; }
            set
            {
                cvUploaded = value;
                RaisePropertyChange("CvUploaded");
                CandidatesView.Refresh();
            }
        }

        private Document documentToAction;
        public Document DocumentToAction
        {
            get { return documentToAction; }
            set
            {
                documentToAction = value;
                RaisePropertyChange("DocumentToAction");
            }
        }


        private bool allProjectsSelected;
        public bool AllProjectsSelected
        {
            get { return allProjectsSelected; }
            set
            {
                allProjectsSelected = value;
                RaisePropertyChange("AllProjectsSelected");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }

            }
        }

        private bool allAreasSelected;
        public bool AllAreasSelected
        {
            get { return allAreasSelected; }
            set
            {
                allAreasSelected = value;
                RaisePropertyChange("AllAreasSelected");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }

            }
        }

        private bool allCompaniesSelected;
        public bool AllCompaniesSelected
        {
            get { return allCompaniesSelected; }
            set
            {
                allCompaniesSelected = value;
                RaisePropertyChange("AllCompaniesSelected");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }

            }
        }

        private bool companiesListOpened;
        public bool CompaniesListOpened
        {
            get { return companiesListOpened; }
            set
            {
                companiesListOpened = value;
                RaisePropertyChange("CompaniesListOpened");
            }
        }

        private Candidate selectedCandidate;
        public Candidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set
            {
                selectedCandidate = value;
                RaisePropertyChange("SelectedCandidate");
            }
        }

        #endregion
       
        #region Commands
        public ICommand SelectAllProjectsCommand { get; set; }
        public ICommand SelectAllAreasCommand { get; set; }
        public ICommand SelectAllCompaniesCommand { get; set; }
        public ICommand ProjectSelectionChangeCommand { get; set; }
        public ICommand CompanySelectionChangeCommand { get; set; }
        public ICommand ImportFileCommand { get; set; }
        public ICommand AreaSelectionChangeCommand { get; set; }
        public ICommand ResetFiltersCommand { get; set; }
        public ICommand CompaniesListOpenedChangeCommand { get; set; }
        public ICommand SelectCandidateChangeCommand { get; set; }
        public ICommand AddNewCandidateCommand { get; set; }
        #endregion

        #region methodsForCommands
        #region SelectAllProjectsCommand
        private bool CanSelectAllProjects(object obj)
        {
            return true;
        }
        private void SelectAllProjects(object obj)
        {
            bool isChecked = (bool)obj;
            //((CheckBox)obj).IsChecked;
            ConfigProjectsLibs.ToList().ForEach(e => e.Selected = (bool)isChecked);
            AllProjectsSelected = (bool)isChecked;
        }
        #endregion

        #region SelectAllAreasCommandSel

        private bool CanSelectAllAreas(object obj)
        {
            return true;
        }
        private void SelectAllAreas(object obj)
        {
            bool isChecked = (bool)obj;
            //((CheckBox)obj).IsChecked;
            ConfigAreaCollection.ToList().ForEach(e => e.Selected = (bool)isChecked);
            AllAreasSelected = (bool)isChecked;
        }

        #endregion

        #region SelectAllCompaniesCommand

        private bool CanSelectAllCompanies(object obj)
        {
            return true;
        }
        private void SelectAllCompanies(object obj)
        {
            bool isChecked = (bool)obj;
            //((CheckBox)obj).IsChecked;
            ConfigCompanyCollection.ToList().ForEach(e => e.Selected = (bool)isChecked);
            AllCompaniesSelected = (bool)isChecked;
        }

        #endregion

        #region ProjectSelectionChangeCommand
        private bool CanProjectSelectionChange(object obj)
        {
            return true;
        }
        private void ProjectSelectionChange(object obj)
        {

            if (ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count())
            {
                AllProjectsSelected = true;
            }
            else
            {
                AllProjectsSelected = false;
            }

        }
        #endregion

        #region AreaSelectionChangeCommand
        private bool CanAreaSelectionChange(object obj)
        {
            return true;
        }
        private void AreaSelectionChange(object obj)
        {
            if (ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count())
            {
                AllAreasSelected = true;
            }
            else
            {
                AllAreasSelected = false;
            }

        }

        #endregion

        #region ResetFiltersCommand

        private void ResetFilters(object obj)
        {
            CombinedRefreshNeeded = false;
            FirstNameFilter = null;
            LastNameFilter = null;
            ProjectNameFilter = null;
            AreaNameFilter = null;
            PositionNameFilter = null;
            CompanyNameFilter = null;
            AllProjectsSelected = true;
            AllAreasSelected = true;
            AllCompaniesSelected = true;
            SelectAllProjects(AllProjectsSelected);
            SelectAllAreas(AllAreasSelected);
            SelectAllCompanies(AllCompaniesSelected);
            ViewRefresh();
            CombinedRefreshNeeded = true;


        }
        private bool CanResetFilters(object obj)
        {
            return true;
        }
        #endregion

        #region CompaniesListOpenedChangeCommand
        private bool CanCompaniesListOpenedChange(object i)
        {
            return true;
        }

        private void CompaniesListOpenedChange(object i)
        {
            CompaniesListOpened = bool.Parse(i.ToString());
        }
        #endregion

        #region ImportFileCommand
        private void ImportFile(object obj)
        {
            DocumentToAction.Action = ActionType.Import;
            MessengerCandidate.Default.Send<Document>(DocumentToAction);
            dialogService.ShowDetailDialog();

            if (DocumentToAction.DocumentNames.Length == 0) { return; }

            try {
                DataTable dt = GlobalFunctions.ImportDataToDataTable(DocumentToAction.DocumentNames[0]);
                if (dt.Select("Candidate is null or Status is null").Count() > 0)
                {
                    MessageBox.Show("Import failed,. Please complete Candidate Name and Status details!");
                    return;
                }
                string fileName = Path.GetFileNameWithoutExtension(DocumentToAction.DocumentNames[0]);
                dt.Columns.Add("fileName");

                foreach (DataRow row in dt.Rows)
                {
                    row["fileName"] = fileName;
                }
                string TempTableName = "##importTable";
                string SqlToCreateTable = @"
            CREATE TABLE ##importTable
            (
                Company NVARCHAR(MAX),	
                Candidate NVARCHAR(MAX),	
                Position NVARCHAR(MAX),
                [Contact phone] NVARCHAR(MAX),	
                [Contact e-mail] NVARCHAR(MAX),
                [Status] NVARCHAR(MAX),	
                [Comments] NVARCHAR(MAX),
                [History of contact] NVARCHAR(MAX),
                fileName NVARCHAR(MAX)
            )
            ";

                DBObjects.UploadFileIntoDB(dt, TempTableName, SqlToCreateTable);
                LoadData();
                addConfigsToCandidates();
                LoadViews();

                //MessengerConfigProject.Default.Send<ConfigProject>(ConfigProjectCollection.FirstOrDefault());
                string newProjectName = fileName.Substring(fileName.IndexOf("-") + 1, fileName.Length - (fileName.IndexOf("-") + 1)).Substring(0, fileName.Substring(fileName.IndexOf("-") + 1, fileName.Length - (fileName.IndexOf("-") + 1)).IndexOf('_', fileName.Substring(fileName.IndexOf("-") + 1, fileName.Length - (fileName.IndexOf("-") + 1)).IndexOf('_') + 1)).Trim();
                configProjectMessenger.RaiseConfigProjectChanged(ConfigProjectCollection.Where(e => e.ProjectName == newProjectName).FirstOrDefault());
                    
                    
                MessageBox.Show("import success");
            }

            catch (Exception ex)
            {

            }
           
          
        }
        private bool CanImportFile(object obj)
        {
            return true;
        }
        #endregion

        #region CompanySelectionChangeCommand
        private bool CanCompanySelectionChange(object obj)
        {
            return true;
        }
        private void CompanySelectionChange(object obj)
        {

            if (ConfigCompanyCollectionView.Cast<ConfigCompany>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigCompanyCollectionView.SourceCollection.Cast<ConfigCompany>().ToList().Count())
            {
                AllCompaniesSelected = true;
            }
            else
            {
                AllCompaniesSelected = false;
            }

        }
        #endregion

        #region SelectCandidateChangeCommand
        private bool CanSelectCandidateChange(object o)
        {
            if (SelectedCandidate != null)
            {
                return true;
            }
            return false;
        }
        private void SelectCandidateChange(object o)
        {
            Candidate SelectedCandidateTemp = new Candidate();
            GlobalFunctions.CopyProperties(SelectedCandidate, SelectedCandidateTemp);
            MessengerCandidate.Default.Send<Candidate>(SelectedCandidate);
        }       
        #endregion
       
        #endregion

        #region collections

        private ObservableCollection<Candidate> candidates;
        public ObservableCollection<Candidate> Candidates
        {
            get { return candidates; }
            set
            {
                candidates = value;
                RaisePropertyChange("Candidates");
            }
        }

        private CollectionView candidatesView;
        public CollectionView CandidatesView
        {
            get { return candidatesView; }
            set {
                candidatesView = value;
                RaisePropertyChange("CandidatesView");
                }
        }

       
        private ObservableCollection<ConfigArea> configAreaCollection;
        public ObservableCollection<ConfigArea> ConfigAreaCollection
        {
            get { return configAreaCollection; }
            set
            {
                configAreaCollection = value;
                RaisePropertyChange("ConfigAreaCollection");
            }
        }

        private CollectionView configAreaView;
        public CollectionView ConfigAreaView
        {
            get { return configAreaView; }
            set
            {
                configAreaView = value;
                RaisePropertyChange("ConfigAreaView");
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
               // RaisePropertyChange("ConfigCompanyCollectionView");
                //ConfigCompanyCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigCompanyCollection);
                //ConfigCompanyCollectionView.Filter = ConfigCompanyFilter;
            }
        }

        private CollectionView configCompanyCollectionView;
        public CollectionView ConfigCompanyCollectionView
        {
            get { return configCompanyCollectionView; }
            set
            {
                configCompanyCollectionView = value;
                RaisePropertyChange("ConfigCompanyCollectionView");
            }
        }

        private ObservableCollection<CandidateCompany> candidateCompanyCollection;
        public ObservableCollection<CandidateCompany> CandidateCompanyCollection
        {
            get { return candidateCompanyCollection; }
            set
            {
                candidateCompanyCollection = value;
                RaisePropertyChange("CandidateCompanyCollection");
            }
        }

        private ObservableCollection<ConfigCompanyProject> configCompanyProjectCollection;
        public ObservableCollection<ConfigCompanyProject> ConfigCompanyProjectCollection
        {
            get { return configCompanyProjectCollection; }
            set
            {
                configCompanyProjectCollection = value;
                RaisePropertyChange("ConfigCompanyProjectCollection");
            }
        }


        private ObservableCollection<ConfigProjectsLib> configProjectsLibs;
        public ObservableCollection<ConfigProjectsLib> ConfigProjectsLibs
        {
            get { return configProjectsLibs; }
            set
            {
                configProjectsLibs = value;
                RaisePropertyChange("ConfigProjectsLibs");
            }
        }

        private CollectionView configProjectsLibsView;
        public CollectionView ConfigProjectsLibsView
        {
            get { return configProjectsLibsView; }
            set
            {
                configProjectsLibsView = value;
                RaisePropertyChange("ConfigProjectsLibsView");
                if (CandidatesView != null)
                {
                    CandidatesView.Refresh();
                }

            }
        }

        private ObservableCollection<ConfigProjectCandidate> configProjectCandidateCollection;
        public ObservableCollection<ConfigProjectCandidate> ConfigProjectCandidateCollection
        {
            get { return configProjectCandidateCollection; }
            set
            {
                configProjectCandidateCollection = value;
                RaisePropertyChange("ConfigProjectCollection");
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

        #endregion


        public MainListViewModel(ICandidateRepository CandidateRepository, IConfigProjectsLibRepository configProjectsLibRepository,
            IConfigProjectsCandidateRepository configProjectsCandidateRepository, IConfigAreaRepository configAreaRepository,
            IConfigProjectRepository configProjectRepository, IConfigCompanyRepository configCompanyRepository,
            ICandidateCompanyRepository candidateCompanyRepository,
            IConfigCompanyProjectRepository configCompanyProjectRepository, IDialogService dialogService)
        {
            this.candidateRepository = CandidateRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectsCandidateRepository = configProjectsCandidateRepository;
            this.configAreaRepository = configAreaRepository;
            this.configProjectRepository = configProjectRepository;
            this.configCompanyRepository = configCompanyRepository;
            this.candidateCompanyRepository = candidateCompanyRepository;
            this.configCompanyProjectRepository = configCompanyProjectRepository;
            this.dialogService = dialogService;
            configProjectMessenger = ConfigProjectMessenger.Instance;
            configProjectMessenger.ConfigProjectChanged += OnConfigProjectChanged;
            DocumentToAction = new Document();
            AllProjectsSelected = true;
            AllAreasSelected = true;
            AllCompaniesSelected = true;
            CombinedRefreshNeeded = true;

            App.splashScreen.AddMessage("Loading data");
            //App.splashScreen.UpdateProgress(0);
            #region loadData           
            try
            {
                LoadData();
        


            }
            catch (Exception e)
            {

            }
            #endregion
           

            App.splashScreen.AddMessage("Loading views");
            #region loadView
            try
            {
                LoadViews();
            }
            catch (Exception e)
            {

            }
            #endregion
            //App.splashScreen.UpdateProgress(70);
            App.splashScreen.AddMessage("Matching data");
            addConfigsToCandidates();
            App.splashScreen.AddMessage("Matching data completed");
            //App.splashScreen.UpdateProgress(90);
            #region commandsInitialisation
            App.splashScreen.AddMessage("commandsInitialisation");
            CommandsInitialisation();
            App.splashScreen.AddMessage("commandsInitialisation completed");
            //App.splashScreen.UpdateProgress(95);
            #endregion

            MessengerCandidate.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);           
            MessengerCompany.Default.Register<List<CandidateCompany>>(this, OnCompanyReceived);
            //MessengerConfigProject.Default.Register<UpdateListMessageConfigProject>(this, OnConfigProjectReceived);
            MessengerCandidateCollection.Default.Register<ObservableCollection<Candidate>>(this, OnCandidateCollectionReceived);
            MessengerDocument.Default.Register<UpdateDocument>(this, OnUpdateDocumentMessageReceived);
            //MessengerConfigProject.Default.Register<ConfigProject>(this, OnConfigProjectReceived);

            App.splashScreen.AddMessage("Done!");
            //App.splashScreen.UpdateProgress(100);

            App.splashScreen.LoadComplete();
        }

        private void OnConfigProjectChanged(object sender, ConfigProjectValueChangedEventArgs e)
        {
            var receivedConfigProject = e.ConfigProject;
            if(!ConfigProjectsLibs.Any(cp=>cp.Id==receivedConfigProject.ConfigProjectLibID))
            {
                ConfigProjectsLibs.Add(new ConfigProjectsLib
                                {   Id = receivedConfigProject.ConfigProjectLibID,
                                ProjectName = receivedConfigProject.ProjectName,
                                Selected = true }
                                    );
                ConfigProjectCollection.Add(receivedConfigProject);
            }
            else
            {
                ConfigProject configProject = ConfigProjectCollection.Where(pr => pr.ID == receivedConfigProject.ID).FirstOrDefault();
                ConfigProjectCollection.Remove(configProject);
                ConfigProjectCollection.Add(receivedConfigProject);
                ConfigProjectCandidateCollection.Where(cc => cc.ConfigProjectID == receivedConfigProject.ID)
                                                .ToList()
                                                .ForEach(cc =>
                                                {
                                                    cc.Area = receivedConfigProject.AreaName;
                                                    cc.ConfigAreaID = receivedConfigProject.ConfigAreaID;
                                                });
                addConfigsToCandidates();
                //Candidates.ToList().ForEach(cc=>cc.CandidateProjects=)

            }
           
        }

        private void OnConfigProjectReceived(ConfigProject obj)
        {
            
        }

        private void OnConfigProjectReceived(UpdateListMessageConfigProject obj)
        {
           
        }

        private void OnUpdateDocumentMessageReceived(UpdateDocument obj)
        {
            dialogService.CloseDetailDialog();
        }

        private void OnCandidateCollectionReceived(ObservableCollection<Candidate> candidateCollection)
        {
            if(candidateCollection.Any(e=>e.IsNew))
            {
                dialogService.CloseDetailDialog();
                 Candidate candidate = candidateCollection.Where(e => e.IsNew).FirstOrDefault();

                candidate.IsNew = false;
                //configProjectsCandidateRepository.AddConfigProjectCandidate()
                candidate.CandidateProjects.Add
                    (new ConfigProjectCandidate()
                    {
                        ConfigProjectID= ConfigProjectCollection
                                .Join(ConfigProjectsLibs
                                     .Where(s => s.ProjectName == "Not assigned").ToList(),
                                     c => c.ConfigProjectLibID, s => s.Id, (s, c) => s.ID).FirstOrDefault(),
                        CompanyId= ConfigCompanyCollection.Where(c => c.Company == "Not assigned").FirstOrDefault().ID,
                        Company= "Not assigned",
                        ProjectName= "Not assigned",
                        Position="",
                        ConfigCandidateID=candidate.ID
                    }               
                    );
                candidate.CandidateProjects[0].ID = configProjectsCandidateRepository.
                    AddConfigProjectCandidate(candidate.ID, ConfigProjectCollection
                                .Join(ConfigProjectsLibs
                                     .Where(s => s.ProjectName == "Not assigned").ToList(),
                                  c => c.ConfigProjectLibID, s => s.Id, (s, c) => s).FirstOrDefault(), "", candidate.CandidateProjects[0].CompanyId);

                
            }
              


        }

        private void OnCompanyReceived(List<CandidateCompany> companylist)
        {

            foreach (var CandidateCompany in companylist.Where(e => !ConfigCompanyCollection.Any(cc=>cc.ID==e.ID)))
            {
                ConfigCompanyCollection.Add(new ConfigCompany { ID = CandidateCompany.ID, Company = CandidateCompany.Company });
            }
        
            ConfigCompanyCollection = ConfigCompanyCollection.OrderBy(e => e.Company).ToObservableCollection();
            ConfigCompanyCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigCompanyCollection);
            ConfigCompanyCollectionView.Filter = ConfigCompanyFilter;
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();

        }

        private void LoadViews()
        {
            ConfigAreaView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigAreaCollection);
            ConfigAreaView.Filter = ConfigAreaFilter;

            ConfigProjectsLibsView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigProjectsLibs);
            ConfigProjectsLibsView.Filter = ConfigProjectsLibsFilter;
            ConfigCompanyCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigCompanyCollection);
            ConfigCompanyCollectionView.Filter = ConfigCompanyFilter;
            CandidatesView = (CollectionView)CollectionViewSource.GetDefaultView(Candidates);
            CandidatesView.Filter = CandidatesViewFilter;

        }
        private void LoadData()
        {
            try
            {
                Candidates = candidateRepository.GetCandidates();
                ConfigProjectsLibs = configProjectsLibRepository.GetConfigProjectsLibs();
                ConfigProjectCandidateCollection = configProjectsCandidateRepository.GetConfigProjects();
                ConfigAreaCollection = configAreaRepository.GetConfigAreas();
                ConfigProjectCollection = configProjectRepository.GetConfigProjects();
                ConfigCompanyCollection = configCompanyRepository.GetConfigCompanys();
                ConfigCompanyProjectCollection = configCompanyProjectRepository.GetConfigCompanyProjects();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                //return;
            }


        }

        private void CommandsInitialisation()
        {
            ImportFileCommand = new CustomCommand(ImportFile, CanImportFile);
            SelectAllProjectsCommand = new CustomCommand(SelectAllProjects, CanSelectAllProjects);
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
            AreaSelectionChangeCommand = new CustomCommand(AreaSelectionChange, CanSelectAllAreas);
            SelectAllAreasCommand = new CustomCommand(SelectAllAreas, CanSelectAllAreas);
            ResetFiltersCommand = new CustomCommand(ResetFilters, CanResetFilters);
            CompaniesListOpenedChangeCommand = new CustomCommand(CompaniesListOpenedChange, CanCompaniesListOpenedChange);
            SelectAllCompaniesCommand = new CustomCommand(SelectAllCompanies, CanSelectAllCompanies);
            CompanySelectionChangeCommand = new CustomCommand(CompanySelectionChange, CanCompanySelectionChange);
            SelectCandidateChangeCommand = new CustomCommand(SelectCandidateChange, CanSelectCandidateChange);
            AddNewCandidateCommand = new CustomCommand(AddNewCandidate, CanAddNewCandidate);
        }

        private bool CanAddNewCandidate(object obj)
        {
            return true;
        }

        private void AddNewCandidate(object obj)
        {
            MessengerCandidateCollection.Default.Send<ObservableCollection<Candidate>>(Candidates);
            dialogService.ShowAddNewCandidateDialog();
           
        }

        private void addConfigsToCandidates()
        {
            Candidates.ToList()
                      .ForEach(e => e.CandidateProjects = ConfigProjectCandidateCollection.Where(cp => cp.ConfigCandidateID.Equals(e.ID)).ToList().ToObservableCollection());
            
        }

        #region Filters

        private void ViewRefresh()
        {
            if (ConfigAreaView != null)
            {
                ConfigAreaView.Refresh();
            }

            if (ConfigProjectsLibsView != null)
            {
                ConfigProjectsLibsView.Refresh();
            }

            if (ConfigCompanyCollectionView != null)
            {
                ConfigCompanyCollectionView.Refresh();
            }

            if (CandidatesView != null)
            {
                CandidatesView.Refresh();
            }

        }

        private bool CandidatesViewFilter(object item)
        {
            if (string.IsNullOrEmpty(FirstNameFilter)
                && CvUploaded == null
                &&
                string.IsNullOrEmpty(LastNameFilter)
                && string.IsNullOrEmpty(PositionNameFilter)
                && string.IsNullOrEmpty(PhoneNumberFilter)
                && ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                && ConfigCompanyCollectionView.Cast<ConfigCompany>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigCompanyCollectionView.SourceCollection.Cast<ConfigCompany>().ToList().Count()
                )
            {

                return true;
            }
            else
            {
                if (
                    (string.IsNullOrEmpty(FirstNameFilter) || (((Candidate)item).FirstName.ToLower()).StartsWith(FirstNameFilter.ToLower()))
                 && (string.IsNullOrEmpty(LastNameFilter) || (((Candidate)item).LastName.ToLower()).StartsWith(LastNameFilter.ToLower()))
                  && (string.IsNullOrEmpty(PhoneNumberFilter) ||
                  (((Candidate)item).FirstPhone.ToLower().Trim()).StartsWith(PhoneNumberFilter.ToLower().Trim()) ||
                  (((Candidate)item).SecondPhone.ToLower().Trim()).StartsWith(PhoneNumberFilter.ToLower().Trim())
                  )
                   && (CvUploaded == null || CvUploaded.Equals(((Candidate)item).CvUploaded))

                 &&

                 (
                     (
                     ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                     && ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                     && ConfigCompanyCollectionView.Cast<ConfigCompany>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigCompanyCollectionView.SourceCollection.Cast<ConfigCompany>().ToList().Count()
                     )
                     ||
                     (
                     ((Candidate)item).CandidateProjects.Join((ConfigProjectsLibsView.Cast<ConfigProjectsLib>().Where(e => e.Selected.Equals(true))).ToList(), cp => cp.ConfigProjectLibID, cpl => cpl.Id, (cp, cpl) => cp.ConfigCandidateID).ToList().Count > 0
                     &&
                     ((Candidate)item).CandidateProjects.Join((ConfigCompanyCollectionView.Cast<ConfigCompany>().Where(e => e.Selected.Equals(true))).ToList(), cc => cc.CompanyId, ccc => ccc.ID, (cc, ccc) => cc).ToList().Count > 0

                     )

                    )
                    &&
                      (string.IsNullOrEmpty(PositionNameFilter)
                     ||
                     ((Candidate)item).CandidateProjects.Where(e => e.Position.ToLower().Contains(PositionNameFilter.ToLower())).Count() > 0
                     )

                   )
                {
                    return true;
                }

                return false;
            }
        }

        private bool ConfigProjectsLibsFilter(object item)
        {
            if (string.IsNullOrEmpty(ProjectNameFilter) &&

                ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                )
            {
                return true;
            }
            else
            {
                if ((string.IsNullOrEmpty(ProjectNameFilter) || ((ConfigProjectsLib)item).ProjectName.ToLower().Contains(ProjectNameFilter.ToLower()))
                  &&
                    (
                    ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count() ||
                    ConfigProjectCollection.Join((ConfigAreaView.Cast<ConfigArea>().Where(e => e.Selected.Equals(true))).ToList(), cp => cp.ConfigAreaID, ca => ca.ID, (cp, ca) => cp).Where(cp => cp.ConfigProjectLibID.Equals(((ConfigProjectsLib)item).Id)).ToList().Count() > 0
                    )
                  )


                {
                    return true;
                }
                return false;
            }

        }

        private bool ConfigCompanyFilter(object item)
        {

            if (string.IsNullOrEmpty(CompanyNameFilter) &&

                 ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                &&
                 ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                )
            {
                return true;
            }
            else
            {
                if ((string.IsNullOrEmpty(CompanyNameFilter) || ((ConfigCompany)item).Company.ToLower().StartsWith(CompanyNameFilter.ToLower()))
                  &&
                    (
                       (
                        ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                        &&
                        ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                        )
                    ||
                  ConfigProjectCollection.
                  Join((ConfigProjectsLibsView.Cast<ConfigProjectsLib>().Where(e => e.Selected.Equals(true))).ToList(), cp => cp.ConfigProjectLibID, cpl => cpl.Id, (cp, cpl) => cp).
                  Join(ConfigCompanyProjectCollection, cp => cp.ID, ccp => ccp.ProjectID, (cp, ccp) => ccp).ToList().Where(ccp => ccp.CompanyID.Equals(((ConfigCompany)item).ID)).ToList().Count > 0

                   )
                  )


                {
                    return true;
                }
                return false;
            }

        }

        private bool ConfigAreaFilter(object item)
        {
            if (string.IsNullOrEmpty(AreaNameFilter))
            {
                return true;
            }
            else
            {
                if (((ConfigArea)item).AreaName.ToLower().StartsWith(AreaNameFilter.ToLower()))
                {
                    return true;
                }
                return false;
            }

        }



        private string firstNameFilter;
        public string FirstNameFilter
        {
            get { return firstNameFilter; }
            set
            {
                firstNameFilter = value;
                RaisePropertyChange("FirstNameFilter");
                CandidatesView.Refresh();
            }
        }

        private string lastNameFilter;
        public string LastNameFilter
        {
            get { return lastNameFilter; }
            set
            {
                lastNameFilter = value;
                RaisePropertyChange("LastNameFilter");
                if (CombinedRefreshNeeded)
                {
                    CandidatesView.Refresh();
                }

            }
        }

        private string positionNameFilter;
        public string PositionNameFilter
        {
            get { return positionNameFilter; }
            set
            {
                positionNameFilter = value;
                RaisePropertyChange("PositionNameFilter");


                if (CombinedRefreshNeeded)
                {
                    CandidatesView.Refresh();
                }

            }
        }


        private string projectNameFilter;
        public string ProjectNameFilter
        {
            get { return projectNameFilter; }
            set
            {
                projectNameFilter = value;
                RaisePropertyChange("ProjectNameFilter");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }

            }
        }

        private string areaNameFilter;
        public string AreaNameFilter
        {
            get { return areaNameFilter; }
            set
            {
                areaNameFilter = value;
                RaisePropertyChange("AreaNameFilter");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }

            }
        }

        private string companyNameFilter;
        public string CompanyNameFilter
        {
            get { return companyNameFilter; }
            set
            {
                companyNameFilter = value;
                RaisePropertyChange("CompanyNameFilter");
                if (CombinedRefreshNeeded)
                {
                    ViewRefresh();
                }
            }
        }

        private string phoneNumberFilter;
        public string PhoneNumberFilter
        {
            get { return phoneNumberFilter; }
            set
            {
                phoneNumberFilter = value;
                RaisePropertyChange("PhoneNumberFilter");
                if (CombinedRefreshNeeded)
                {
                    CandidatesView.Refresh();
                }
            }
        }


        #endregion

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
