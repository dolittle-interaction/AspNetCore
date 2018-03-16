Dolittle.namespace("Dolittle.views", {
    UriMapperPathResolver: Dolittle.views.PathResolver.extend(function () {
        this.canResolve = function (element, path) {
            var closest = $(element).closest("[data-urimapper]");
            if (closest.length === 1) {
                var mapperName = $(closest[0]).data("urimapper");
                if (Dolittle.uriMappers[mapperName].hasMappingFor(path) === true) {
                    return true;
                }
            }
            return Dolittle.uriMappers.default.hasMappingFor(path);
        };

        this.resolve = function (element, path) {
            var closest = $(element).closest("[data-urimapper]");
            if (closest.length === 1) {
                var mapperName = $(closest[0]).data("urimapper");
                if (Dolittle.uriMappers[mapperName].hasMappingFor(path) === true) {
                    return Dolittle.uriMappers[mapperName].resolve(path);
                }
            }
            return Dolittle.uriMappers.default.resolve(path);
        };
    })
});
if (typeof Dolittle.views.pathResolvers !== "undefined") {
    Dolittle.views.pathResolvers.UriMapperPathResolver = Dolittle.views.UriMapperPathResolver;
}