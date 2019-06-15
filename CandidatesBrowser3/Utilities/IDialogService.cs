using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Utilities
{
    public interface IDialogService
    {
        void CloseDetailDialog();
        void ShowDetailDialog();
        void ShowAssignProjectDialog();
        void ShowAddNewCandidateDialog();
    }
}
