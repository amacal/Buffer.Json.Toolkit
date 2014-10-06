namespace Buffer.Json
{
    public class Options : IOptions
    {
        private readonly string[] args;

        public Options(string[] args)
        {
            this.args = args;
        }

        public bool Apply(IConfigurable target)
        {
            bool stop = false;
            string verb = null;
            string parameter = null;

            foreach (string arg in this.args)
            {
                if (verb == null)
                {
                    Command command = new Command(verb = arg);
                    target.SetCommand(command);
                    stop |= command.Stop.IsSet;
                }

                if (stop == false)
                {
                    if (parameter == null)
                    {
                        parameter = arg;
                    }
                    else
                    {
                        Parameter value = new Parameter(arg);

                        switch (parameter)
                        {
                            case "--input":
                                target.SetInput(value);
                                stop |= value.Stop.IsSet;
                                break;

                            case "--output":
                                target.SetOutput(value);
                                stop |= value.Stop.IsSet;
                                break;
                        }
                    }
                }

                if (stop == true)
                {
                    break;
                }
            }

            if (stop == true || verb == null)
            {
                target.ShowUsage();
                return false;
            }

            return true;
        }
    }
}
