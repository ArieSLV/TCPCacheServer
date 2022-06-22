using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCPCacheServer.Builders
{
    class GetCommandBuilder : CommandBuilder
    {
        public GetCommandBuilder()
        {
            command = new Command();
            BuildCommandType(CommandType.Get);
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
            throw new InvalidOperationException("There is no value field on Get Command");
        }
    
    }
}