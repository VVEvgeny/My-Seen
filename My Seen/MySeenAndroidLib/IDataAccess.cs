using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeenMobileWebViewLib
{
    public interface IDataAccess
    {
        IEnumerable<FilmsView> LoadFilms();
        IEnumerable<SerialsView> LoadSerials();

        bool isFilmNameExist(string Name);
        void AddFilm(string Name, int Genre, int Rating);
        void GetFilmById(int id, ref string name, ref int genre, ref int rating);

        bool isSerialNameExist(string Name);
        void AddSerial(string Name, int Season,int Series, int Genre, int Rating);
    }
}
