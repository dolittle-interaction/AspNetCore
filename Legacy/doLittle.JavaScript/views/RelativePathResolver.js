Dolittle.namespace("Dolittle.views", {
    RelativePathResolver: Dolittle.views.PathResolver.extend(function () {
        this.canResolve = function (element, path) {
            var closest = $(element).closest("[data-view]");
            if (closest.length === 1) {
                var view = $(closest[0]).view;

            }
            return false;
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
    Dolittle.views.pathResolvers.RelativePathResolver = Dolittle.views.RelativePathResolver;
}