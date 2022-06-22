using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCPCacheServer.Builders
{
    public abstract class CommandBuilder
    {
        public Command command;

        public Command Command
        {
            get { return command; }
        }

        public abstract void BuildCommandType(CommandType commandType);
        public abstract void BuildCommandKey(string key);
        public abstract void BuildCommandValue(string value);

        public Command BuildCommand()
        {
            return command;
        }



    }
}