using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CandidatesBrowser3.DAL;
using System.Windows.Data;
using System.Windows.Input;
using CandidatesBrowser3.Utilities;
using System.Windows.Controls;
using CandidatesBrowser3.Extensions;

namespace CandidatesBrowser3.ViewModel
{
   public class MainListViewModel:INotifyPropertyChanged
    {
        #region properties
        private ICandidateRepository candidateRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectsRepository configProjectsRepository;

        private bool allProjectsSelected;
        public bool AllProjectsSelected
        {
            get { return allProjectsSelected; }
            set {
                allProjectsSelected = value;
                RaisePropertyChange("AllProjectsSelected");
                ViewRefresh();
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
            public ICommand ProjectSelectionChangeCommand { get; set; }
            public ICommand ResetFiltersCommand { get; set; }
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
            #region ResetFiltersCommand
                private void ResetFilters(object obj)
                {
                    FirstNameFilter = null;
                    LastNameFilter = null;
                    ProjectNameFilter = null;
                    AllProjectsSelected = true;          
                    SelectAllProjects(AllProjectsSelected);
                    
                }
                private bool CanResetFilters(object obj)
                {
                    return true;
                }
            #endregion
        #endregion
        #region collections
        private ObservableCollection<Candidate> candidates;
        public ObservableCollection<Candidate> Candidates
        {
            get { return candidates; }
            set {
                candidates = value;
                RaisePropertyChange("Candidates");
            }
        }



        private ObservableCollection<ConfigProjectsLib> configProjectsLibs;
        public ObservableCollection<ConfigProjectsLib> ConfigProjectsLibs
        {
            get { return configProjectsLibs; }
            set {
                configProjectsLibs = value;
                RaisePropertyChange("ConfigProjectsLibs");
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


        public CollectionView CandidatesView { get; set; }

        private CollectionView configProjectsLibsView;
        public CollectionView ConfigProjectsLibsView
        {
            get { return configProjectsLibsView; }
            set {
                configProjectsLibsView = value;
                RaisePropertyChange("ConfigProjectsLibsView");
                if (CandidatesView!=null)
                {
                    CandidatesView.Refresh();
                }
               
            }
        }


        #endregion




        public MainListViewModel(ICandidateRepository CandidateRepository, IConfigProjectsLibRepository configProjectsLibRepository,
            IConfigProjectsRepository configProjectsRepository)
        {
            this.candidateRepository = CandidateRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectsRepository = configProjectsRepository;
            AllProjectsSelected = true;
            #region loadData           
            try
            {
                LoadData();
                AddProjectsTocandidate();
            }
            catch (Exception e)
            {

            }
            #endregion
            #region loadView
            try
            {
                LoadViews();
            }
            catch (Exception e)
            {

            }
            #endregion

            #region commandsInitialisation
            SelectAllProjectsCommand = new CustomCommand(SelectAllProjects, CanSelectAllProjects);
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
            ResetFiltersCommand = new CustomCommand(ResetFilters, CanResetFilters);
            #endregion
        }

        private void LoadViews()
        {
            ConfigProjectsLibsView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigProjectsLibs);
            ConfigProjectsLibsView.Filter = ConfigProjectsLibsFilter;
            CandidatesView = (CollectionView)CollectionViewSource.GetDefaultView(Candidates);
            CandidatesView.Filter = CandidatesViewFilter;

        }
        private void LoadData()
        {
            Candidates = candidateRepository.GetCandidates();
            ConfigProjectsLibs = configProjectsLibRepository.GetConfigProjectsLibs();
            ConfigProjectCollection = configProjectsRepository.GetConfigProjects();
      
        }

        private void AddProjectsTocandidate()
        {        
            Candidates.ToList().ForEach(e => e.CandidateProjects = ConfigProjectCollection.Where(cp => cp.ConfigCandidateID.Equals(e.ID)).ToList().ToObservableCollection());
        }

        #region Filters

        private void ViewRefresh()
        {

            if (ConfigProjectsLibsView!=null)
            {
                ConfigProjectsLibsView.Refresh();
            }
            if (CandidatesView != null)
            {
                CandidatesView.Refresh();
            }
                
        }

        private bool CandidatesViewFilter(object item)
        {
            if (string.IsNullOrEmpty(FirstNameFilter)
                &&
                string.IsNullOrEmpty(LastNameFilter)
               

                && ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                )
            {
                return true;
            }
            else
            {
                if (
                    (string.IsNullOrEmpty(FirstNameFilter) || (((Candidate)item).FirstName.ToLower()).StartsWith(FirstNameFilter.ToLower()))
                 && (string.IsNullOrEmpty(LastNameFilter) || (((Candidate)item).LastName.ToLower()).StartsWith(LastNameFilter.ToLower()))
                 && (
                     ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                     || ((Candidate)item).CandidateProjects.Join((ConfigProjectsLibsView.Cast<ConfigProjectsLib>().Where(e => e.Selected.Equals(true))).ToList(), cp=>cp.ConfigProjectLibID,cpl=>cpl.Id,(cp,cpl)=>cp.ConfigCandidateID).ToList().Count>0
                   
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
            if (string.IsNullOrEmpty(ProjectNameFilter))
            {
                return true;
            }
            else
            {
                if (((ConfigProjectsLib)item).ProjectName.ToLower().StartsWith(ProjectNameFilter.ToLower()))
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
                CandidatesView.Refresh();
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
                ViewRefresh();
            }
        }
        #endregion

        public void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
