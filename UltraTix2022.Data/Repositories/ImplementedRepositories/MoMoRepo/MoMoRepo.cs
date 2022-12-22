using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.MoMoRepo
{
    public class MoMoRepo : Repository<MoMoResponse>, IMoMoRepo
    {
        public MoMoRepo(UltraTixDBContext context) : base(context)
        {
        }
    }
}

