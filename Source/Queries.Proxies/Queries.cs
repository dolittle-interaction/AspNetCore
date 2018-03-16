using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Queries.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Dolittle/Queries/Proxies")]
    public class ReadModels : ControllerBase
    {
        readonly IQueryProxies _proxies;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxies"></param>
        public ReadModels(IQueryProxies proxies)
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