﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Linq;

namespace FriendOrganizer.UI.ViewModel
{
	public class NavigationViewModel : ViewModelBase, INavigationViewModel
	{
		private IFriendLookupDataService _friendLookupService;
		private IEventAggregator _eventAgrregator;

		public NavigationViewModel(IFriendLookupDataService friendLookupService, IEventAggregator eventAggregator)
		{
			_friendLookupService = friendLookupService;
			_eventAgrregator = eventAggregator;
			Friends = new ObservableCollection<NavigationItemViewModel>();
			_eventAgrregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
		}

		private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
		{
			var lookupItem = Friends.Single(l => l.Id == obj.Id);
			lookupItem.DisplayMember = obj.DisplayMember;
		}

		public async Task LoadAsync()
		{
			var lookup = await _friendLookupService.GetFriendLookupAsync();
			Friends.Clear();
			foreach (var item in lookup)
			{
				Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
			}
		}

		public ObservableCollection<NavigationItemViewModel> Friends { get; }

		private NavigationItemViewModel _selectedFriend;

		public NavigationItemViewModel SelectedFriend
		{
			get { return _selectedFriend; }
			set
			{
				_selectedFriend = value;
				OnPropertyChanged();
				if (_selectedFriend != null)
				{
					_eventAgrregator.GetEvent<OpenFriendDetailViewEvent>()
						.Publish(_selectedFriend.Id);
				}

			}
		}

	}
}