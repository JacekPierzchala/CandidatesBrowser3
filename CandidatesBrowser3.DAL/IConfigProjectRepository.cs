﻿using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.DAL
{
    public interface IConfigProjectRepository
    {
        void DeleteConfigProject(ConfigProject configProject);
        ConfigProject GetConfigProject();
        ConfigProject ConfigProjectByID(int id);
        ObservableCollection<ConfigProject> GetConfigProjects();
        void UpdateConfigProject(ConfigProject configProject);
        void UpdateConfigProjectDocumentInfo(ConfigProject configProject);

        int AddNewConfigProjects(int ConfigProjectsLib, int ConfigArea);
    }


}