using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeenAndroidLib
{
    public interface IDataAccess
    {
        IEnumerable<FilmsView> LoadFilms();
        IEnumerable<SerialsView> LoadSerials();
    }
}
