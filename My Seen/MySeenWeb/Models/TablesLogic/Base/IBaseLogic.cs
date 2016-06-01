namespace MySeenWeb.Models.TablesLogic.Base
{
    public interface IBaseLogic
    {
        string GetError();
        bool Delete(string id, string userId);
    }
}