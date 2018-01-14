using System.Threading.Tasks;
using Prism.Events;
using FriendOrganizer.UI.Event;
using System;
using System.Windows.Input;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;

namespace FriendOrganizer.UI.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private IEventAggregator _eventAggregator;
		private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
		private IMessageDialogService _messageDialogService;
		private IFriendDetailViewModel _friendDetailViewModel;

		public MainViewModel(INavigationViewModel navigationViewModel, 
			Func<IFriendDetailViewModel> friendDetailViewModelCreator,
			IEventAggregator eventAggregator,
			IMessageDialogService messageDialogService)
		{
			_eventAggregator = eventAggregator;
			NavigationViewModel = navigationViewModel;
			_friendDetailViewModelCreator = friendDetailViewModelCreator;
			_messageDialogService = messageDialogService;

			_eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
				.Subscribe(OnOpenFriendDetailView);

			CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
		}

	

		public async Task LoadAsync()
		{
			await NavigationViewModel.LoadAsync();
		}

		public ICommand CreateNewFriendCommand { get; }
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

		private async void OnOpenFriendDetailView(int? friendId)
		{
			if (FriendDetailViewModel!=null && FriendDetailViewModel.HasChanges)
			{
				var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
				if (result == MessageDialogResult.Cancel)
				{
					return;
				}
			}
			FriendDetailViewModel = _friendDetailViewModelCreator();
			await FriendDetailViewModel.LoadAsync(friendId);
		}

		private void OnCreateNewFriendExecute()
		{
			OnOpenFriendDetailView(null);
		}
	}


}

