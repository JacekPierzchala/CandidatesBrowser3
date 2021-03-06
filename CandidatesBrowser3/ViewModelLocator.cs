﻿using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Utilities;
using CandidatesBrowser3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3
{
    public class ViewModelLocator
    {
        #region fields
      
        #region MainListViewModel
        private static ICandidateRepository candidateRepository = new CandidateRepository();
        private static IConfigProjectsLibRepository configProjectsLibRepository = new ConfigProjectsLibRepository();
        private static IConfigProjectsCandidateRepository configProjectsCandidateRepository = new ConfigProjectsCandidateRepository();
        private static IConfigAreaRepository configAreaRepository = new ConfigAreaRepository();
        private static IConfigProjectRepository configProjectRepository = new ConfigProjectRepository();
        private static IConfigCompanyRepository configCompanyRepository = new ConfigCompanyRepository();
        private static ICandidateCompanyRepository candidateCompanyRepository = new CandidateCompanyRepository();
        private static IConfigCompanyProjectRepository configCompanyProjectRepository = new ConfigCompanyProjectRepository();
        #endregion

        #region CandidateDetailsViewModel
        private static ICandidateHistoryRepository candidateHistoryRepository = new CandidateHistoryRepository();
        private static IDialogService dialogService = new DialogService();
        private static IConfigStatusLibRepository configStatusLibRepository = new ConfigStatusLibRepository();
        #endregion


        #endregion




        private static MainListViewModel mainListViewModel = 
            new MainListViewModel(candidateRepository, configProjectsLibRepository, 
            configProjectsCandidateRepository,configAreaRepository,configProjectRepository, 
            configCompanyRepository,candidateCompanyRepository,configCompanyProjectRepository, dialogService);
        public static MainListViewModel MainListViewModel
        {
            get
            {
                return mainListViewModel;
            }
        }

        private static CandidateDetailsViewModel candidateDetailsViewModel=new CandidateDetailsViewModel(candidateHistoryRepository, dialogService, candidateRepository, configStatusLibRepository);
        public static CandidateDetailsViewModel CandidateDetailsViewModel
        {
            get { return candidateDetailsViewModel; }
            
        }

        private static ProjectSettingsViewModel projectSettingsViewModel = new ProjectSettingsViewModel
            (configProjectsLibRepository: configProjectsLibRepository, configProjectRepository: configProjectRepository,
            configAreaRepository: configAreaRepository);
        public static ProjectSettingsViewModel ProjectSettingsViewModel
        {
            get { return projectSettingsViewModel; }

        }

        private static FileDialogsViewModel fileDialogsViewModel = new FileDialogsViewModel(dialogService);
        public static FileDialogsViewModel FileDialogsViewModel
        {
            get
            {
                return fileDialogsViewModel;
            }
        }
        
        private static AssignNewProjectViewModel assignNewProjectViewModel= 
            new AssignNewProjectViewModel(configStatusLibRepository,configProjectsLibRepository,configProjectRepository,configCompanyRepository,
                dialogService,  candidateCompanyRepository, configProjectsCandidateRepository, candidateHistoryRepository);

        public static AssignNewProjectViewModel AssignNewProjectViewModel
        {
            get { return assignNewProjectViewModel; }
            
        }

        private static ProjectsReportViewModel projectsReportViewModel = new ProjectsReportViewModel(configProjectRepository, candidateHistoryRepository,dialogService);
        public static ProjectsReportViewModel ProjectsReportViewModel
        {
            get { return projectsReportViewModel; }
        }

        private static AddNewCandidateViewModel addNewCandidateViewModel = new AddNewCandidateViewModel(candidateRepository, candidateCompanyRepository);
        public static AddNewCandidateViewModel AddNewCandidateViewModel
        {
            get { return addNewCandidateViewModel; }
        }

    }
}
