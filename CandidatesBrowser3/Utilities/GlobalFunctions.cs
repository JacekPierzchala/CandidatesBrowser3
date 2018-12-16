using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.Utilities
{
    public static class GlobalFunctions
    {
        public static void CopyProperties<T>(T objectToCopy, T objectToPaste)
        {
            foreach(PropertyInfo propertyToGet in objectToCopy.GetType().GetProperties().OrderBy(e=>e.Name))
            {
                foreach(PropertyInfo propertyToSet in objectToPaste.GetType().GetProperties().OrderBy(e => e.Name))
                {
                    if (propertyToGet.Name.Equals(propertyToSet.Name))
                    {
                        propertyToSet.SetValue(objectToPaste, propertyToGet.GetValue(objectToCopy));
                    }

                }
            }

        }
    }
}
