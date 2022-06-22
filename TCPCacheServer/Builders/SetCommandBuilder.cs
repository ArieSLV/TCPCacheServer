using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCPCacheServer.Builders
{
    class SetCommandBuilder : CommandBuilder
    {
        public SetCommandBuilder()
        {
            command = new Command();
            BuildCommandType(CommandType.Set);
        }
    
        public override void BuildCommandKey(string key)
        {
            command.Key = key;
        }

        public override void BuildCommandType(CommandType commandType)
        {
            command.CommandType = commandType;
        }

        public override void BuildCommandValue(string value)
        {
            command.Value = value;
        }
    }
}