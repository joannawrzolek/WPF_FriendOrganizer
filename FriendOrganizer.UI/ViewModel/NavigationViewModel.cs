
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Linq;
using FriendOrganizer.UI.Data.Lookups;

namespace FriendOrganizer.UI.ViewModel
{
	public class NavigationViewModel : ViewModelBase, INavigationViewModel
	{
		private IFriendLookupDataService _friendLookupService;
		private IEventAggregator _eventAgrregator;

		public NavigationViewModel(IFriendLookupDataService friendLookupService,
			IEventAggregator eventAggregator)
		{
			_friendLookupService = friendLookupService;
			_eventAgrregator = eventAggregator;
			Friends = new ObservableCollection<NavigationItemViewModel>();
			_eventAgrregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
			_eventAgrregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
		}

		private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
		{
			var lookupItem = Friends.SingleOrDefault(l => l.Id == obj.Id);
			if (lookupItem == null)
			{
				Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAgrregator));
			}
			else
			{
				lookupItem.DisplayMember = obj.DisplayMember;
			}
		}

		public async Task LoadAsync()
		{
			var lookup = await _friendLookupService.GetFriendLookupAsync();
			Friends.Clear();
			foreach (var item in lookup)
			{
				Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
					_eventAgrregator));
			}
		}

		public ObservableCollection<NavigationItemViewModel> Friends { get; }

		private void AfterFriendDeleted(int friendId)
		{
			var friend = Friends.SingleOrDefault(f => f.Id == friendId);
			if (friend != null)
			{
				Friends.Remove(friend);
			}
		}

	}
}
