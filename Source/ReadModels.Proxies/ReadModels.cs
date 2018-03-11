using Microsoft.AspNetCore.Mvc;

namespace doLittle.AspNetCore.ReadModels.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Dolittle/ReadModels/Proxies")]
    public class ReadModels : ControllerBase
    {
        readonly IReadModelProxies _proxies;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxies"></param>
        public ReadModels(IReadModelProxies proxies)
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