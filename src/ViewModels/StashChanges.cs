using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceGit.ViewModels
{
    public class StashChanges : Popup
    {
        public string Message
        {
            get;
            set;
        }

        public bool HasSelectedFiles
        {
            get;
        }

        public bool IncludeUntracked
        {
            get => _repo.Settings.IncludeUntrackedWhenStash;
            set => _repo.Settings.IncludeUntrackedWhenStash = value;
        }

        public bool OnlyStaged
        {
            get => _repo.Settings.OnlyStagedWhenStash;
            set => _repo.Settings.OnlyStagedWhenStash = value;
        }

        public bool KeepIndex
        {
            get => _repo.Settings.KeepIndexWhenStash;
            set => _repo.Settings.KeepIndexWhenStash = value;
        }

        public StashChanges(Repository repo, List<Models.Change> changes, bool hasSelectedFiles)
        {
            _repo = repo;
            _changes = changes;
            HasSelectedFiles = hasSelectedFiles;

            View = new Views.StashChanges() { DataContext = this };
        }

        public override Task<bool> Sure()
        {
            var jobs = _changes;
            if (!HasSelectedFiles && !IncludeUntracked)
            {
                jobs = new List<Models.Change>();
                foreach (var job in _changes)
                {
                    if (job.WorkTree != Models.ChangeState.Untracked && job.WorkTree != Models.ChangeState.Added)
                    {
                        jobs.Add(job);
                    }
                }
            }

            if (jobs.Count == 0)
                return null;

            _repo.SetWatcherEnabled(false);
            ProgressDescription = $"Stash changes ...";

            return Task.Run(() =>
            {
                var succ = new Commands.Stash(_repo.FullPath).Push(jobs, Message, !HasSelectedFiles && OnlyStaged, KeepIndex);
                CallUIThread(() =>
                {
                    _repo.MarkWorkingCopyDirtyManually();
                    _repo.SetWatcherEnabled(true);
                });
                return succ;
            });
        }

        private readonly Repository _repo = null;
        private readonly List<Models.Change> _changes = null;
    }
}
