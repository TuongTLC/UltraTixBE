using System;
using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRequestRepo
{
	public class ArtistRequestRepo : Repository<ArtistRequest>, IArtistRequestRepo
	{
		public ArtistRequestRepo(UltraTixDBContext context) : base(context)
		{
		}

        public async Task<List<AtistRequestViewModel>> getAllRequest()
        {
            var query = from ar in context.ArtistRequests
                        join u in context.AppUsers
                        on ar.UserId equals u.Id
                        select new { ar, u };
            List<AtistRequestViewModel> list = await query.Select(x => new AtistRequestViewModel()
            {
                Id = x.ar.Id,
                UserID = x.ar.UserId,
                FullName = x.u.FullName,
                Description = x.ar.Description,
                Idnumber = x.ar.Idnumber,
                IdIssueDate = x.ar.IdissueDate,
                Idlocation = x.ar.Idlocation,
                Status = x.ar.Status
            }).ToListAsync();
            return list;
        } 

        public async Task<bool> UpdateToArtist(Guid requestID, string status)
        {
            var check = await Get(requestID);
            if (check == null) return false;
            check.Status = status;
            await Update();
            return true;
        }
    }
}

