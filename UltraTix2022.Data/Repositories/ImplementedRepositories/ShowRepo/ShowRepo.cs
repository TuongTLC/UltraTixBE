using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo
{
    public class ShowRepo : Repository<Show>, IShowRepo
    {

        public ShowRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<bool> CheckShowIsExist(Guid showID)
        {
            return (await Get(showID) != null);
        }

        public async Task<List<ShowViewModel>> GetShowsByStatus(string status)
        {
            var query = from s in context.Shows
                        join t in context.ShowTypes
                        on s.ShowTypeId equals t.Id
                        join o in context.Organizers
                        on s.ShowOrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        join l in context.Locations
                        on s.Id equals l.ShowId
                        join c in context.ShowCategories
                        on s.CategoryId equals c.Id
                        where s.Status.Trim().ToLower().Equals(status.Trim().ToLower())
                        select new { s, t, o, a, l, c };

            List<ShowViewModel> result = await query.Select(x => new ShowViewModel()
            {
                Id = x.s.Id,
                ShowName = x.s.ShowName,
                ImgUrl = x.s.ImgUrl ?? "",
                ShowDescriptionDetail = x.s.ShowDetail ?? "",
                ShowDescription = x.s.ShowDescription ?? "",
                DescriptionImageUrl = x.s.DescriptionImageUrl,
                ShowStartDate = x.s.ShowStartDate,
                ShowEndDate = x.s.ShowEndDate,
                ShowTypeName = x.t.ShowTypeName,
                ShowTypeId = x.t.Id,
                OrganizerName = x.a.Username,
                OrganizerID = x.s.ShowOrganizerId,
                Location = x.l.LocationDescription,
                Status = x.s.Status,
                CategoryID = x.c.Id
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowViewModel>> GetAllShows()
        {
            var query = from s in context.Shows
                        join t in context.ShowTypes
                        on s.ShowTypeId equals t.Id
                        join o in context.Organizers
                        on s.ShowOrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        join l in context.Locations
                        on s.Id equals l.ShowId
                        join c in context.ShowCategories
                        on s.CategoryId equals c.Id
                        select new { s, t, o, a, l, c };

            List<ShowViewModel> result = await query.Select(x => new ShowViewModel()
            {
                Id = x.s.Id,
                ShowName = x.s.ShowName,
                ImgUrl = x.s.ImgUrl ?? "",
                ShowDescription = x.s.ShowDescription ?? "",
                ShowDescriptionDetail = x.s.ShowDetail ?? "",
                DescriptionImageUrl = x.s.DescriptionImageUrl,
                ShowStartDate = x.s.ShowStartDate,
                ShowEndDate = x.s.ShowEndDate,
                ShowTypeName = x.t.ShowTypeName,
                ShowTypeId = x.t.Id,
                OrganizerName = x.a.Username,
                OrganizerID = x.s.ShowOrganizerId,
                Location = x.l.LocationDescription,
                Status = x.s.Status,
                CategoryID = x.c.Id,
                Category = x.c.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowViewModel>> GetShowsByLocation(string location)
        {
            var query = from s in context.Shows
                        join t in context.ShowTypes
                        on s.ShowTypeId equals t.Id
                        join o in context.Organizers
                        on s.ShowOrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        join l in context.Locations
                        on s.Id equals l.ShowId
                        join c in context.ShowCategories
                        on s.CategoryId equals c.Id
                        where l.LocationDescription.ToLower().Trim().Contains(location.ToLower().Trim())
                        select new { s, t, o, a, l, c };

            List<ShowViewModel> result = await query.Select(x => new ShowViewModel()
            {
                Id = x.s.Id,
                ShowName = x.s.ShowName,
                ImgUrl = x.s.ImgUrl ?? "",
                ShowDescription = x.s.ShowDescription ?? "",
                ShowDescriptionDetail = x.s.ShowDetail ?? "",
                DescriptionImageUrl = x.s.DescriptionImageUrl,
                ShowStartDate = x.s.ShowStartDate,
                ShowEndDate = x.s.ShowEndDate,
                ShowTypeName = x.t.ShowTypeName,
                ShowTypeId = x.t.Id,
                OrganizerName = x.a.Username,
                OrganizerID = x.s.ShowOrganizerId,
                Location = x.l.LocationDescription,
                Status = x.s.Status,
                CategoryID = x.c.Id
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowViewModel>> GetShowsByType(string type)
        {
            var query = from s in context.Shows
                        join t in context.ShowTypes
                        on s.ShowTypeId equals t.Id
                        join o in context.Organizers
                        on s.ShowOrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        join l in context.Locations
                        on s.Id equals l.ShowId
                        join c in context.ShowCategories
                        on s.CategoryId equals c.Id
                        where t.ShowTypeName.ToLower().Trim().Equals(type.ToLower().Trim())
                        select new { s, t, o, a, l, c };

            List<ShowViewModel> result = await query.Select(x => new ShowViewModel()
            {
                Id = x.s.Id,
                ShowName = x.s.ShowName,
                ImgUrl = x.s.ImgUrl ?? "",
                ShowDescription = x.s.ShowDescription ?? "",
                ShowDescriptionDetail = x.s.ShowDetail ?? "",
                DescriptionImageUrl = x.s.DescriptionImageUrl,
                ShowStartDate = x.s.ShowStartDate,
                ShowEndDate = x.s.ShowEndDate,
                ShowTypeName = x.t.ShowTypeName,
                ShowTypeId = x.t.Id,
                OrganizerName = x.a.FullName ?? "Đông Tây Promotion",
                OrganizerID = x.s.ShowOrganizerId,
                Location = x.l.LocationDescription,
                Status = x.s.Status,
                CategoryID = x.c.Id
            }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateShow(Show showUpdateModel)
        {
            var show = await Get(showUpdateModel.Id);
            if (show == null) return false;

            show.ShowName = showUpdateModel.ShowName;
            show.ShowTypeId = showUpdateModel.ShowTypeId;
            show.ShowName = showUpdateModel.ShowName;
            show.ShowDescription = showUpdateModel.ShowDescription;
            show.ShowDetail = showUpdateModel.ShowDetail;
            show.Status = Commons.DRAFT;
            show.ShowStartDate = showUpdateModel.ShowStartDate;
            show.ShowEndDate = showUpdateModel.ShowEndDate;
            show.DescriptionImageUrl = showUpdateModel.DescriptionImageUrl;
            show.ImgUrl = showUpdateModel.ImgUrl;
            show.CategoryId = showUpdateModel.CategoryId;

            await Update();

            return true;
        }

        public async Task<bool> UpdateShowStatus(Guid showID, string status)
        {
            var Show = await Get(showID);
            if (Show == null) throw new ArgumentException("Show Not Found");

            Show.Status = status;
            await Update();
            return true;
        }

        public async Task<List<ShowViewModel>> GetShowsJoinedForArtist(Guid artistId)
        {
            var query = from s in context.Shows
                        join t in context.ShowTypes
                        on s.ShowTypeId equals t.Id
                        join o in context.Organizers
                        on s.ShowOrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        join l in context.Locations
                        on s.Id equals l.ShowId
                        join c in context.ShowCategories
                        on s.CategoryId equals c.Id
                        join cmp in context.Campaigns
                        on s.Id equals cmp.ShowId
                        join art in context.Artists
                        on cmp.ArtistId equals art.Id
                        where cmp.ArtistId.Equals(artistId)
                        select new { s, t, o, a, l, c, cmp, art };

            List<ShowViewModel> result = await query.Select(x => new ShowViewModel()
            {
                Id = x.s.Id,
                ShowName = x.s.ShowName,
                ImgUrl = x.s.ImgUrl ?? "",
                ShowDescription = x.s.ShowDescription ?? "",
                ShowDescriptionDetail = x.s.ShowDetail ?? "",
                DescriptionImageUrl = x.s.DescriptionImageUrl,
                ShowStartDate = x.s.ShowStartDate,
                ShowEndDate = x.s.ShowEndDate,
                ShowTypeName = x.t.ShowTypeName,
                ShowTypeId = x.t.Id,
                OrganizerName = x.a.Username,
                OrganizerID = x.s.ShowOrganizerId,
                Location = x.l.LocationDescription,
                Status = x.s.Status,
                CategoryID = x.c.Id
            }).ToListAsync();

            return result;
        }

        public async Task<bool> IsShowBelongToOrganizer(Guid organizerId, Guid showId)
        {
            var show = await Get(showId);

            if (show != null)
            {
                if (show.ShowOrganizerId.Equals(organizerId)) return true;
            }
            return false;
        }

        public async Task<bool> UpdateShowCreationStep(Guid showId, int step)
        {
            var show = await Get(showId);
            if (show == null) throw new ArgumentException("Show Not Found");

            show.Step = step;
            await Update();
            return true;
        }

        public async Task<int> GetShowCreationStep(Guid showId)
        {
            var show = await Get(showId);
            if (show == null) throw new ArgumentException("Show Not Found");

            return show.Step;
        }

        public async Task<DateTime> GetShowCreationDate(Guid showId)
        {
            var show = await Get(showId);
            if (show == null) throw new ArgumentException("Show Not Found");

            return show.CreationDate ?? DateTime.Now;
        }
    }
}
