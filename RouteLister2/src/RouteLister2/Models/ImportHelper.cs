using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public static class ImportHelper
    {
        public static bool ReflectEvaluateObjectProperties<T>(T t1, T t2)
        {
            Type t = t1.GetType();
            var properties = t.GetProperties();


            foreach (var item in properties)
            {
                if (!item.Name.Contains("Id"))
                {
                    if (item.GetValue(t1) != item.GetValue(t2))
                    {
                        return false;
                    }
                }

            }
            return true;
        }
    }
}
