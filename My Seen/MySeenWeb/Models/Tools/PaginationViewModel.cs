using System.Collections.Generic;
using System.Linq;

namespace MySeenWeb.Models.Tools
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int pageNum, int totalRecords, int countInPage)
        {
            Load(pageNum, totalRecords, countInPage);
        }
        private void Load(int pageNum, int totalRecords, int countInPage)
        {
            CurentPage = pageNum;
            if (CurentPage < 1) CurentPage = 1;
            if (totalRecords != 0) LastPage = totalRecords / countInPage;
            else LastPage = 1;
            if (totalRecords % countInPage != 0) LastPage++;
            if (CurentPage > LastPage) CurentPage = LastPage;
            IsFirstPage = CurentPage == 1;
            IsLastPage = LastPage == CurentPage;
            MiddlePage = LastPage / 2;
            if (LastPage % 2 != 0) MiddlePage++;
            IsMiddlePage = MiddlePage == CurentPage;

            //Хочу считать сам странички которые надо отобразить
            var listI = new List<int> {1};
            if (IsFirstPage && totalRecords > 1 && !listI.Contains(2)) listI.Add(2);

            if (!listI.Contains(CurentPage)) listI.Add(CurentPage);
            if ((CurentPage - 1) > 0 && !listI.Contains(CurentPage - 1)) listI.Add(CurentPage - 1);
            if ((CurentPage + 1) < LastPage && !listI.Contains(CurentPage + 1)) listI.Add(CurentPage + 1);

            //Середину не показывать если пересекаемся
            if (!(CurentPage == MiddlePage || CurentPage == (MiddlePage - 1) || CurentPage == (MiddlePage + 1))
                && !(CurentPage - 1 == MiddlePage || CurentPage - 1 == (MiddlePage - 1) || CurentPage - 1 == (MiddlePage + 1))
                && !(CurentPage + 1 == MiddlePage || CurentPage + 1 == (MiddlePage - 1) || CurentPage + 1 == (MiddlePage + 1))
                && CurentPage + 2 != MiddlePage - 1
                && CurentPage - 2 != MiddlePage + 1
                )
            {
                if (!listI.Contains(MiddlePage)) listI.Add(MiddlePage);
                if (!listI.Contains(MiddlePage - 1)) listI.Add(MiddlePage - 1);
                if (!listI.Contains(MiddlePage + 1)) listI.Add(MiddlePage + 1);
            }
            if (!listI.Contains(LastPage)) listI.Add(LastPage);
            if (IsLastPage && !listI.Contains(LastPage - 1)) listI.Add(LastPage - 1);


            //если пропуск между страницами всего 1 которую заменили бы на "..." лучше покажу страничку...
            var iPrev = -5;
            var addList = new List<int>();

            listI.Sort();
            foreach (var i in listI)
            {
                if ((iPrev + 2) == i)
                {
                    addList.Add(iPrev + 1);
                }
                iPrev = i;
            }

            listI.AddRange(addList);
            listI.Sort();
            if (IsFirstPage && IsLastPage)listI.Clear();

            List = listI.Distinct();

            SkipRecords = (CurentPage - 1)*countInPage;
        }
        public bool IsFirstPage { get; set; }
        public bool IsMiddlePage { get; set; }
        public bool IsLastPage { get; set; }
        public int CurentPage { get; set; }
        public int LastPage { get; set; }
        public int MiddlePage { get; set; }
        public int SkipRecords { get; set; }

        public IEnumerable<int> List { get; set; }
    }
}
