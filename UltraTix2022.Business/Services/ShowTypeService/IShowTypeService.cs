using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowType;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowTypeService
{
    public interface IShowTypeService
    {
        public Task<bool> InsertShowType(string token, ShowTypeRequestInsertModel showType);
        public Task<List<ShowTypeViewModel>> GetShowType(string token);
    }
}
