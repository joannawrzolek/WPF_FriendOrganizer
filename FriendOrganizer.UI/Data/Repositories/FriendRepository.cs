using System;
using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.DataAccess;
using System.Threading.Tasks;
using System.Data.Entity;
using FriendOrganizer.UI.Data.Repositories;

namespace FriendOrganizer.UI.Data
{
	public class FriendRepository : IFriendRepository
	{

		private FriendOrganizerDbContext _context;

		public FriendRepository(FriendOrganizerDbContext context)
		{
			_context = context;
		}
		public async Task<Friend> GetByIdAsync(int friendId)
		{
			return await _context.Friends.SingleAsync(f => f.Id == friendId);
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
