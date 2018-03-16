Dolittle.namespace("Dolittle.views", {
    PathResolver: Dolittle.Type.extend(function () {
        this.canResolve = function (element, path) {
            return false;
        };

        this.resolve = function (element, path) {

        };
    })
});