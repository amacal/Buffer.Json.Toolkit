using System;

namespace Buffer.Json
{
    public struct Stop : IStop
    {
        public bool IsSet;
        public string Description;

        bool IStop.IsSet
        {
            get { return this.IsSet; }
        }

        bool IStop.HasDescription()
        {
            return String.IsNullOrWhiteSpace(this.Description) == false;
        }

        public string GetDescription()
        {
            return this.Description;
        }
    }
}
