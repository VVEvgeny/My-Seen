using System.Collections.Generic;

namespace MySeenMobileWebViewLib
{
    public interface IDataAccess
    {
        IEnumerable<FilmsView> LoadFilms();
        IEnumerable<SerialsView> LoadSerials();

        bool IsFilmNameExist(string name);
        void AddFilm(string name, int genre, int rating);
        void GetFilmById(int id, ref string name, ref int genre, ref int rating);

        bool IsSerialNameExist(string name);
        void AddSerial(string name, int season,int series, int genre, int rating);
    }
}
