﻿using CandidatesBrowser3.Model;
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
using System.Windows;

namespace CandidatesBrowser3.ViewModel
{
   public class MainListViewModel:INotifyPropertyChanged
    {
        #region properties
        private ICandidateRepository candidateRepository;
        private IConfigProjectsLibRepository configProjectsLibRepository;
        private IConfigProjectsCandidateRepository configProjectsCandidateRepository;
        private IConfigAreaRepository configAreaRepository;
        private IConfigProjectRepository configProjectRepository;
        private IConfigCompanyRepository configCompanyRepository;

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

        private bool allAreasSelected;
        public bool AllAreasSelected
        {
            get { return allAreasSelected; }
            set {
                allAreasSelected = value;
                RaisePropertyChange("AllAreasSelected");
                ViewRefresh();

                }
        }

        private bool companiesListOpened;
        public bool CompaniesListOpened
        {
            get { return companiesListOpened; }
            set {
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
            public ICommand ProjectSelectionChangeCommand { get; set; }
            public ICommand AreaSelectionChangeCommand { get; set; }
            public ICommand ResetFiltersCommand { get; set; }
            public ICommand CompaniesListOpenedChangeCommand { get; set; }
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

             #region SelectAllAreasCommand

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
                        FirstNameFilter = null;
                        LastNameFilter = null;
                        ProjectNameFilter = null;
                        AreaNameFilter = null;
                        AllProjectsSelected = true;
                        AllAreasSelected = true;       
                        SelectAllProjects(AllProjectsSelected);
                        SelectAllAreas(AllAreasSelected);
                    
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
                   CompaniesListOpened= bool.Parse(i.ToString());
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
        public CollectionView CandidatesView { get; set; }

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

        private ObservableCollection<ConfigCompany> configCompanyCollection;
        public ObservableCollection<ConfigCompany> ConfigCompanyCollection
        {
            get { return configCompanyCollection; }
            set {
                configCompanyCollection = value;
                RaisePropertyChange("ConfigCompanyCollection");
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

        private CollectionView configCompanyCollectionView;
        public CollectionView ConfigCompanyCollectionView
        {
            get { return configCompanyCollectionView; }
            set {
                configCompanyCollectionView = value;
                RaisePropertyChange("ConfigCompanyCollectionView");
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
            set {
                configProjectCollection = value;
                RaisePropertyChange("ConfigProjectCollection");
                }
        }

        #endregion




        public MainListViewModel(ICandidateRepository CandidateRepository, IConfigProjectsLibRepository configProjectsLibRepository,
            IConfigProjectsCandidateRepository configProjectsCandidateRepository, IConfigAreaRepository configAreaRepository,
            IConfigProjectRepository configProjectRepository, IConfigCompanyRepository configCompanyRepository)
        {
            this.candidateRepository = CandidateRepository;
            this.configProjectsLibRepository = configProjectsLibRepository;
            this.configProjectsCandidateRepository = configProjectsCandidateRepository;
            this.configAreaRepository = configAreaRepository;
            this.configProjectRepository = configProjectRepository;
            this.configCompanyRepository = configCompanyRepository;

            AllProjectsSelected = true;
            AllAreasSelected = true;
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
            AreaSelectionChangeCommand = new CustomCommand(AreaSelectionChange, CanSelectAllAreas);
            SelectAllAreasCommand = new CustomCommand(SelectAllAreas, CanSelectAllAreas);
            ResetFiltersCommand = new CustomCommand(ResetFilters, CanResetFilters);
            CompaniesListOpenedChangeCommand = new CustomCommand(CompaniesListOpenedChange, CanCompaniesListOpenedChange);
            #endregion
        }

        private void LoadViews()
        {
            ConfigAreaView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigAreaCollection);
            ConfigAreaView.Filter = ConfigAreaFilter;

            ConfigProjectsLibsView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigProjectsLibs);
            ConfigProjectsLibsView.Filter = ConfigProjectsLibsFilter;
            CandidatesView = (CollectionView)CollectionViewSource.GetDefaultView(Candidates);
            CandidatesView.Filter = CandidatesViewFilter;
            ConfigCompanyCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ConfigCompanyCollection);
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
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.Message);
                //return;
            }


        }

        private void AddProjectsTocandidate()
        {        
            Candidates.ToList().ForEach(e => e.CandidateProjects = ConfigProjectCandidateCollection.Where(cp => cp.ConfigCandidateID.Equals(e.ID)).ToList().ToObservableCollection());
        }

        #region Filters

        private void ViewRefresh()
        {            
            if (ConfigAreaView != null)
            {
                ConfigAreaView.Refresh();
            }   
                
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
                     (ConfigProjectsLibsView.Cast<ConfigProjectsLib>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigProjectsLibsView.SourceCollection.Cast<ConfigProjectsLib>().ToList().Count()
                     && ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                     )
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
            if (string.IsNullOrEmpty(ProjectNameFilter) &&
                
                ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()
                )
            {
                return true;
            }
            else
            {
                if ((string.IsNullOrEmpty(ProjectNameFilter)|| ((ConfigProjectsLib)item).ProjectName.ToLower().StartsWith(ProjectNameFilter.ToLower()))
                  &&
                    (
                    ConfigAreaView.Cast<ConfigArea>().ToList().Where(e => e.Selected.Equals(true)).Count() == ConfigAreaView.SourceCollection.Cast<ConfigArea>().ToList().Count()||
                    ConfigProjectCollection.Join((ConfigAreaView.Cast<ConfigArea>().Where(e => e.Selected.Equals(true))).ToList(), cp => cp.ConfigAreaID, ca => ca.ID, (cp, ca) => cp).Where(cp => cp.ConfigProjectLibID.Equals(((ConfigProjectsLib)item).Id)).ToList().Count()>0
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

        private string areaNameFilter;
        public string AreaNameFilter
        {
            get { return areaNameFilter; }
            set
            {
                areaNameFilter = value;
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
