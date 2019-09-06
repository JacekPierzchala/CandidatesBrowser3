using System;
using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandidatesBrowser3.Model;
using System.Collections.ObjectModel;
using CommonUlitlities;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace CandidatesBrowser3.ViewModel
{
    public class ProjectSettingsViewModel : INotifyPropertyChanged
    {
        private IConfigAreaRepository configAreaRepository;
        private IConfigProjectRepository configProjectRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private ConfigProjectMessenger configProjectMessenger;

        #region Collections
        private ObservableCollection<ConfigArea> configAreasCollection;
        public ObservableCollection<ConfigArea> ConfigAreasCollection
        {
            get { return configAreasCollection; }
            set
            {
                configAreasCollection = value;
                RaisePropertyChange("ConfigAreasCollection");
            }
        }

        private ObservableCollection<ConfigProject> configProjectsCollection;
        public ObservableCollection<ConfigProject> ConfigProjectsCollection
        {
            get { return configProjectsCollection; }
            set
            {
                configProjectsCollection = value;
                RaisePropertyChange("ConfigProjectsCollection");
            }
        }

        public CollectionView ProjectsCollectionView { get; set; }

        #endregion

        #region Properties

        public ICommand SelectProjectChangeCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AddProjectCommand { get; set; }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set {
                projectName = value;
                RaisePropertyChange("ProjectName");
                ProjectsCollectionView.Refresh();
            }
        }

        private string newProjectName;
        public string NewProjectName
        {
            get { return newProjectName; }
            set
            {
                newProjectName = value;
                RaisePropertyChange("NewProjectName");

            }
        }


        private ConfigProject selectedProject;
        public ConfigProject SelectedProject
        {
            get { return selectedProject; }
            set {
                selectedProject = value;
                RaisePropertyChange("SelectedProject");
            }
        }

        private ConfigArea selectedArea;
        public ConfigArea SelectedArea
        {
            get { return selectedArea; }
            set
            {
                selectedArea = value;
                RaisePropertyChange("SelectedArea");
            }
        }

        #endregion

        #region commandsMethods



        #region SaveCommand
        private bool CanSave(object obj)
        {
            return SelectedProject != null && SelectedArea != null;
        }

        private void Save(object obj)
        {
            SelectedProject.AreaName = SelectedArea.AreaName;
            SelectedProject.ConfigAreaID = SelectedArea.ID;
            configProjectMessenger.RaiseConfigProjectChanged(ConfigProjectsCollection.Where(e => e.ID == SelectedProject.ID).FirstOrDefault());
            //configprojectRepository update
        }
        #endregion

        #region SelectProjectChangeCommand
        private void SelectProjectChange(object obj)
        {
            SelectedArea = ConfigAreasCollection.Where(e => e.ID == SelectedProject.ConfigAreaID).FirstOrDefault();
        }
        private bool CanSelectProjectChange(object obj)
        {
            return SelectedProject != null;
        }
        #endregion

        #region AddProjectCommand
        private void AddProject(object obj)
        {
            if (ConfigProjectsCollection.Where(e=>e.ProjectName.ToLower().Equals(NewProjectName.ToLower())).Any())
            {
                MessageBox.Show("Provided project already exists!");
                return;
            }

            var newConfigProjectsLibId = configProjectsLibRepository.AddNewConfigProjectsLib(NewProjectName);
            //nweConfigAreaid configArearepositoer.where.name==notAssigned
            var newConfigProjectId= configProjectRepository.AddNewConfigProjects(newConfigProjectsLibId, ConfigAreasCollection.Where(e => e.AreaName.Equals("Not assigned")).FirstOrDefault().ID);
            //ad new configprojectid (newConfigProjectsLibId, nweConfigAreaid)
            ConfigProjectsCollection.Add
                (
                new ConfigProject()
                {
                    ID = newConfigProjectId,

                    AreaName = "Not assigned",
                    ConfigAreaID = ConfigAreasCollection.Where(e => e.AreaName.Equals("Not assigned")).FirstOrDefault().ID,
                    ConfigProjectLibID = newConfigProjectsLibId,
                    ProjectName = NewProjectName,
                    JdUploaded = false,
                    Selected = true
                }
                );
            ConfigProjectsCollection = ConfigProjectsCollection.OrderBy(e => e.ProjectName).ToObservableCollection();
            
            ConfigProjectsLib configProjectsLib = new ConfigProjectsLib()
            { Id = newConfigProjectsLibId, ProjectName = NewProjectName, Selected = true };
             ///send congifdPorjecr
            configProjectMessenger.RaiseConfigProjectChanged(ConfigProjectsCollection.Where(e => e.ID == newConfigProjectId).FirstOrDefault());

        }

        private bool CanAddProject(object obj)
        {
            return !string.IsNullOrWhiteSpace(NewProjectName);
        }
        #endregion
        #endregion

        public ProjectSettingsViewModel(IConfigAreaRepository  configAreaRepository, IConfigProjectRepository configProjectRepository,
            IConfigProjectsLibRepository configProjectsLibRepository)
        {
            this.configAreaRepository = configAreaRepository;
            this.configProjectRepository = configProjectRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            configProjectMessenger = ConfigProjectMessenger.Instance;
            loadData();

            SelectProjectChangeCommand = new CustomCommand(SelectProjectChange, CanSelectProjectChange);
            AddProjectCommand = new CustomCommand(AddProject, CanAddProject);
            SaveCommand = new CustomCommand(Save, CanSave);
           // MessengerConfigProject.Default.Register<ConfigProject>(this, OnConfigProjectReceived);
            //MessengerConfigProject.Default.Register<UpdateListMessageConfigProject>(this, OnConfigProjectUpdated);


        }

      




        private void  loadData()
        {
            ConfigProjectsCollection = configProjectRepository.GetConfigProjects().OrderBy(e=>e.ProjectName).ToObservableCollection(); ;
            ConfigAreasCollection = configAreaRepository.GetConfigAreas().OrderBy(e=>e.AreaName).ToObservableCollection();

            ProjectsCollectionView=(CollectionView)CollectionViewSource.GetDefaultView(ConfigProjectsCollection);
            ProjectsCollectionView.Filter = projectNameFilter;
        }


        #region filters

        private bool projectNameFilter(object item)
        {
            if (string.IsNullOrEmpty(ProjectName))
            {
                return true;
            }
            else
            {
                if (((ConfigProject)item).ProjectName.ToLower().Contains(ProjectName.ToLower()))
                {
                    return true;
                }
                return false;
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
