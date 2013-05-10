using OTWB.Coordinates;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB.Extensions
{
    public static class Conversions
    {
        public static PathFragment TransformFragment<TSource>(this IEnumerable<TSource> sourceItems, Func<TSource, ICoordinate> transformFunction) 
        {
            PathFragment targetItems = new PathFragment();

            foreach (TSource sourceItem in sourceItems)
            {
                targetItems.Add(transformFunction(sourceItem));
            }

            return targetItems;
        }
    }
}
