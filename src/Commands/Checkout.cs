using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGit.Commands
{
    public class Checkout : Command
    {
        public Checkout(string repo)
        {
            WorkingDirectory = repo;
            Context = repo;
        }

        public bool Branch(string branch, Action<string> onProgress)
        {
            Args = $"checkout --recurse-submodules --progress {branch}";
            TraitErrorAsOutput = true;
            _outputHandler = onProgress;
            return Exec();
        }

        public bool Branch(string branch, string basedOn, Action<string> onProgress)
        {
            Args = $"checkout --recurse-submodules --progress -b {branch} {basedOn}";
            TraitErrorAsOutput = true;
            _outputHandler = onProgress;
            return Exec();
        }

        public bool UseTheirs(List<string> files)
        {
            var builder = new StringBuilder();
            builder.Append("checkout --theirs --");
            foreach (var f in files)
            {
                builder.Append(" \"");
                builder.Append(f);
                builder.Append("\"");
            }
            Args = builder.ToString();
            return Exec();
        }

        public bool UseMine(List<string> files)
        {
            var builder = new StringBuilder();
            builder.Append("checkout --ours --");
            foreach (var f in files)
            {
                builder.Append(" \"");
                builder.Append(f);
                builder.Append("\"");
            }
            Args = builder.ToString();
            return Exec();
        }

        public bool FileWithRevision(string file, string revision)
        {
            Args = $"checkout --no-overlay {revision} -- \"{file}\"";
            return Exec();
        }

        public bool Commit(string commitId, Action<string> onProgress)
        {
            Args = $"checkout --detach --progress {commitId}";
            TraitErrorAsOutput = true;
            _outputHandler = onProgress;
            return Exec();
        }

        protected override void OnReadline(string line)
        {
            _outputHandler?.Invoke(line);
        }

        private Action<string> _outputHandler;
    }
}
