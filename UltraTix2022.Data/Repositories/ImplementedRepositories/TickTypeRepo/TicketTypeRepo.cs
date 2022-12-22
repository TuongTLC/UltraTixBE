using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.TicketType;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo
{
    public class TicketTypeRepo : Repository<TicketType>, ITicketTypeRepo
    {
        public TicketTypeRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<double> GetLowestPrice(Guid ShowID)
        {
            var query = from t in context.TicketTypes
                        where t.ShowId.Equals(ShowID)
                        select new { t };

            List<TicketTypeViewModel> result = await query.Select(x => new TicketTypeViewModel()
            {
                Id = x.t.Id,
                TicketTypeName = x.t.TicketTypeName,
                OriginalPrice = x.t.OriginalPrice,
            }).ToListAsync();

            var lowesrPrice = result.Min(p => p.OriginalPrice);

            return lowesrPrice;
        }

        public async Task<string> GetNameByID(Guid ID)
        {
            var ticketType = await Get(ID);
            return ticketType.TicketTypeName;
        }

        public async Task<List<TicketTypeViewModel>> GetTicketTypesByShowID(Guid ShowID)
        {
            var query = from t in context.TicketTypes
                        where t.ShowId.Equals(ShowID)
                        select new { t };

            List<TicketTypeViewModel> result = await query.Select(x => new TicketTypeViewModel()
            {
                Id = x.t.Id,
                TicketTypeName = x.t.TicketTypeName,
                OriginalPrice = x.t.OriginalPrice,
                Quantity = x.t.Quantity,
                ShowId = x.t.ShowId,
                TicketTypeDescription = x.t.TicketTypeName,
                TicketTypeDiscount = x.t.TicketTypeDiscount,
                UnitSold = x.t.UnitSold
            }).ToListAsync();

            return result;
        }

        public async Task<List<TicketTypeChartView>> GetTicketTypesForChartView(Guid ShowID)
        {
            var query = from t in context.TicketTypes
                        join s in context.Shows
                        on t.ShowId equals s.Id
                        where t.ShowId.Equals(ShowID)
                        select new { t, s };
            List<TicketTypeChartView> list = await query.Select(x=> new TicketTypeChartView()
            {
                TicketTypeName = x.t.TicketTypeName,
                TicketTypePrice = x.t.OriginalPrice,
                TicketTypeUnitSold = x.t.UnitSold,
                TicketTypeUnitSoldNormal = x.t.TicketTypeNormalUnitSold,
                TicketTypeUnitSoldArtist =x.t.TicketTypeViaLinkUnitSold

            }).ToListAsync();
            return list;
        }

        public async Task<bool> RemoveTicketType(Guid ticketTypeID)
        {
            var ticket = await Get(ticketTypeID);
            if (ticket != null)
            {
                context.TicketTypes.Remove(ticket);
                await Update();
                return true;
            }
            return false;
        }
        /*
        public async Task<bool> UpdateTicketType(TicketTypeRequestUpdateModel ticketType)
        {
            var ticketTypeEnt = await Get(ticketType.ID);
            if (ticketTypeEnt != null)
            {
                ticketTypeEnt.TicketTypeName = ticketType.TicketTypeName;
                ticketTypeEnt.TicketTypeDescription = ticketType.TicketTypeDescription;
                ticketTypeEnt.TicketTypeDiscount = ticketType.TicketTypeDiscount;
                ticketTypeEnt.Quantity = ticketType.Quantity;
                ticketTypeEnt.OriginalPrice = ticketType.OriginalPrice;
                await Update();
                return true;
            }
            return false;
        }
        */
        public async Task<int> UpdateUnitSold(Guid ticketTypeID, int unitSold, bool isBuyViaLink)
        {
            var ticketTypeEntity = await Get(ticketTypeID);
            if (ticketTypeEntity != null)
            {
                ticketTypeEntity.UnitSold += unitSold;

                switch (isBuyViaLink)
                {
                    case true:
                        {
                            if (ticketTypeEntity.TicketTypeViaLinkUnitSold == null)
                            {
                                ticketTypeEntity.TicketTypeViaLinkUnitSold = 0;
                            }
                            ticketTypeEntity.TicketTypeViaLinkUnitSold += unitSold;
                            break;
                        }
                    case false:
                        {
                            if (ticketTypeEntity.TicketTypeNormalUnitSold == null)
                            {
                                ticketTypeEntity.TicketTypeNormalUnitSold = 0;
                            }
                            ticketTypeEntity.TicketTypeNormalUnitSold += unitSold;
                            break;
                        }
                }

                await Update();
                return ticketTypeEntity.UnitSold;
            }
            return -1;
        }

        public async Task<int> ReOpenUnitSold(Guid ticketTypeID, int unitSold, bool isBuyViaLink)
        {
            var ticketTypeEntity = await Get(ticketTypeID);
            if (ticketTypeEntity != null)
            {
                ticketTypeEntity.UnitSold -= unitSold;

                switch (isBuyViaLink)
                {
                    case true:
                        {
                            if (ticketTypeEntity.TicketTypeViaLinkUnitSold == null)
                            {
                                ticketTypeEntity.TicketTypeViaLinkUnitSold = 0;
                            }
                            ticketTypeEntity.TicketTypeViaLinkUnitSold -= unitSold;
                            break;
                        }
                    case false:
                        {
                            if (ticketTypeEntity.TicketTypeNormalUnitSold == null)
                            {
                                ticketTypeEntity.TicketTypeNormalUnitSold = 0;
                            }
                            ticketTypeEntity.TicketTypeNormalUnitSold -= unitSold;
                            break;
                        }
                }

                await Update();
                return ticketTypeEntity.UnitSold;
            }
            return -1;
        }
    }
}
