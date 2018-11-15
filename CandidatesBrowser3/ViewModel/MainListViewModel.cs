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

namespace CandidatesBrowser3.ViewModel
{
   public class MainListViewModel:INotifyPropertyChanged
    {
        #region properties
        private ICandidateRepository candidateRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        #endregion
        #region Commands
        public ICommand SelectAllProjectsCommand { get; set; }

        #endregion
        #region methodsForCommands
        private bool CanSelectAllProjects(object obj)
        {
            return true;
        }
        
        private void SelectAllProjects(object obj)
        {
            bool? isChecked = ((CheckBox)obj).IsChecked;
            ConfigProjectsLibs.ToList().ForEach(e => e.Selected = (bool)isChecked);           
        }
        #endregion

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


        private Candidate selectedCandidate;
        public Candidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set {
                selectedCandidate = value;
                RaisePropertyChange("SelectedCandidate");
            }
        }

        public CollectionView CandidatesView { get; set; }
        public CollectionView ConfigProjectsLibsView { get; set; }

        public MainListViewModel(ICandidateRepository CandidateRepository, IConfigProjectsLibRepository configProjectsLibRepository)
        {
            this.candidateRepository = CandidateRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            #region loadData           
            try
            {
                LoadData();
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
            #endregion
        }

        private void LoadViews()
        {
            CandidatesView = (CollectionView)CollectionViewSource.GetDefaultView(Candidates);
            CandidatesView.Filter = CandidatesViewFilter;
            ConfigProjectsLibsView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigProjectsLibs);
            ConfigProjectsLibsView.Filter = ConfigProjectsLibsFilter;
        }
        private void LoadData()
        {
            Candidates = candidateRepository.GetCandidates();
            ConfigProjectsLibs = configProjectsLibRepository.GetConfigProjectsLibs();
      
        }


        #region Filters
        private bool CandidatesViewFilter(object item)
        {
            if (string.IsNullOrEmpty(FirstNameFilter)
                &&
                string.IsNullOrEmpty(LastNameFilter))
            {
                return true;
            }
            else
            {
                if (
                    (string.IsNullOrEmpty(FirstNameFilter) || (((Candidate)item).FirstName.ToLower()).StartsWith(FirstNameFilter.ToLower()))
                 && (string.IsNullOrEmpty(LastNameFilter) || (((Candidate)item).LastName.ToLower()).StartsWith(LastNameFilter.ToLower()))
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
                ConfigProjectsLibsView.Refresh();
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
