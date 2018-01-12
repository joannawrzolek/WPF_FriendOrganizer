using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Events;
using FriendOrganizer.UI.Event;
using System;

namespace FriendOrganizer.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private IEventAggregator _eventAggregator;
		private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;

		private IFriendDetailViewModel _friendDetailViewModel;

		public MainViewModel(INavigationViewModel navigationViewModel, 
			Func<IFriendDetailViewModel> friendDetailViewModelCreator,
			IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			NavigationViewModel = navigationViewModel;
			_friendDetailViewModelCreator = friendDetailViewModelCreator;

			_eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
				.Subscribe(OnOpenFriendDetailView);
		}

		public async Task LoadAsync()
		{
			await NavigationViewModel.LoadAsync();
		}

	

		public INavigationViewModel NavigationViewModel { get; }

	

		public IFriendDetailViewModel FriendDetailViewModel
		{
			get { return _friendDetailViewModel; }
			private set
			{
				_friendDetailViewModel = value; 
				OnPropertyChanged();
			}
		}



		private async void OnOpenFriendDetailView(int friendId)
		{
			FriendDetailViewModel = _friendDetailViewModelCreator();
			await FriendDetailViewModel.LoadAsync(friendId);
		}
	}


}

