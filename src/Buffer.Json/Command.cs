namespace Buffer.Json
{
    public class Command : ICommand
    {
        private Stop stop;
        private readonly string verb;

        public Command(string verb)
        {
            this.verb = verb;
        }

        public string Verb
        {
            get { return this.verb; }
        }

        public IStop Stop
        {
            get { return this.stop; }
        }

        void ICommand.Stop()
        {
            this.stop = new Stop();
        }
    }
}
