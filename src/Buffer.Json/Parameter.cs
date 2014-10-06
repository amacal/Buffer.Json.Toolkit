namespace Buffer.Json
{
    public class Parameter : IParameter
    {
        private Stop stop;
        private readonly string value;

        public Parameter(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }

        public IStop Stop
        {
            get { return this.stop; }
        }

        void IParameter.Stop()
        {
            this.stop = new Stop();
        }

        void IParameter.StopWith(string message)
        {
            this.stop = new Stop();
        }
    }
}
