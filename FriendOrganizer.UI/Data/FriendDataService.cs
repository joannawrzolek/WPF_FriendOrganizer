﻿using System;
using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.DataAccess;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FriendOrganizer.UI.Data
{
	public class FriendDataService : IFriendDataService
	{
		private Func<FriendOrganizerDbContext> _contextCreator;

		public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
		{
			_contextCreator = contextCreator;
		}
		public async Task<Friend> GetByIdAsync(int friendId)
		{
			using (var ctx = _contextCreator())
			{
				return await ctx.Friends.AsNoTracking().SingleAsync(f=>f.Id == friendId);
			}
		}

		public async Task SaveAsync(Friend friend)
		{
			using (var ctx = _contextCreator())
			{
				ctx.Friends.Attach(friend);
				ctx.Entry(friend).State = EntityState.Modified;
				await ctx.SaveChangesAsync();
			}
		}
	}
}
