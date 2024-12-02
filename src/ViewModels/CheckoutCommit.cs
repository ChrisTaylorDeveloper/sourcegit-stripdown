using System.Threading.Tasks;

namespace SourceGit.ViewModels
{
    public class CheckoutCommit : Popup
    {
        public Models.Commit Commit
        {
            get;
        }

        public bool AutoStash
        {
            get => _autoStash;
            set => SetProperty(ref _autoStash, value);
        }

        public CheckoutCommit(Repository repo, Models.Commit commit)
        {
            _repo = repo;
            Commit = commit;
            View = new Views.CheckoutCommit() { DataContext = this };
        }

        public override Task<bool> Sure()
        {
            _repo.SetWatcherEnabled(false);
            ProgressDescription = $"Checkout Commit '{Commit.SHA}' ...";

            return Task.Run(() =>
            {
                var changes = new Commands.CountLocalChangesWithoutUntracked(_repo.FullPath).Result();
                var needPopStash = false;
                if (changes > 0)
                {
                    if (AutoStash)
                    {
                        SetProgressDescription("Stash local changes ...");
                        var succ = new Commands.Stash(_repo.FullPath).Push("CHECKOUT_AUTO_STASH");
                        if (!succ)
                        {
                            CallUIThread(() => _repo.SetWatcherEnabled(true));
                            return false;
                        }

                        needPopStash = true;
                    }
                    else
                    {
                        SetProgressDescription("Discard local changes ...");
                        Commands.Discard.All(_repo.FullPath, false);
                    }
                }

                SetProgressDescription("Checkout commit ...");
                var rs = new Commands.Checkout(_repo.FullPath).Commit(Commit.SHA, SetProgressDescription);

                if (needPopStash)
                {
                    SetProgressDescription("Re-apply local changes...");
                    rs = new Commands.Stash(_repo.FullPath).Pop("stash@{0}");
                }

                CallUIThread(() => _repo.SetWatcherEnabled(true));
                return rs;
            });
        }

        private readonly Repository _repo = null;
        private bool _autoStash = true;
    }
}
