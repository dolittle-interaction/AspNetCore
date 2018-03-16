using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Commands.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Dolittle/Commands/Proxies")]
    public class Commands : ControllerBase
    {
        ICommandProxies _proxies;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxies"></param>
        public Commands(ICommandProxies proxies)
        {
            _proxies = proxies;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return _proxies.Generate();
        }
        
    }
}