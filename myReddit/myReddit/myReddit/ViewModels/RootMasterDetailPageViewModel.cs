using System.Collections.Generic;
using System.Linq;
using myReddit.Helpers;
using myReddit.Models;
using myReddit.Navigation;
using myReddit.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace myReddit.ViewModels
{
public class RootMasterDetailPageViewModel : BindableBase, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IRedditApiSource _redditApiSource;

    private IList<Grouping<string, Subreddit>> _menuGroupings;
    private Subreddit _currentSubreddit;

    public RootMasterDetailPageViewModel(INavigationService navigationService, IRedditApiSource redditApiSource)
    {
        _navigationService = navigationService;
        _redditApiSource = redditApiSource;

        ChangeSubredditCommand = DelegateCommand<Subreddit>.FromAsyncHandler(async (subReddit) =>
        {
            var navParams = new NavigationParameters
            {
                {"currentSubreddit", subReddit}
            };
            CurrentSubreddit = subReddit;
            await _navigationService.NavigateAsync($"{PageTokens.RootNavigationPage}/{PageTokens.PostsPage}", navParams);
            // or
            //await _navigationService.NavigateAsync($"{PageTokens.RootNavigationPage}/{PageTokens.PostsPage}?subredditTitle={subReddit.Title}");            
        });
    }

    public DelegateCommand<Subreddit> ChangeSubredditCommand { get; private set; }

    public Subreddit CurrentSubreddit
    {
        get { return _currentSubreddit; }
        set { SetProperty(ref _currentSubreddit, value); }
    }

    public IList<Grouping<string, Subreddit>> MenuGroupings
    {
        get { return _menuGroupings; }
        set { SetProperty(ref _menuGroupings, value); }
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
        // NOP
    }

    public async void OnNavigatedTo(NavigationParameters parameters)
    {
        if (MenuGroupings != null)
        {
            return;
        }
        var subReddits = await _redditApiSource.GetSubredditsAsync();
        MenuGroupings = GetMenuGroupings(subReddits);
        CurrentSubreddit = MenuGroupings.FirstOrDefault()?.Values.FirstOrDefault();
        await ChangeSubredditCommand.Execute(_currentSubreddit);
    }

    private static IList<Grouping<string, Subreddit>> GetMenuGroupings(IList<Subreddit> subReddits)
    {
        return new List<Grouping<string, Subreddit>>
        {
            new Grouping<string, Subreddit>("SUBREDDITS", subReddits)
        };
    }
}
}
