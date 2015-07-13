using System;
using System.Threading.Tasks;

namespace FDA.Node
{
    /// <summary>
    /// Entry point for Edge.js
    /// </summary>
    public class Startup
    {
        public async Task<object> Start()
        {
            return true;
        }

        public async Task<object> Close()
        {
            return true;
        }
    }
}
