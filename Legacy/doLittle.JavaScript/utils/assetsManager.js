Dolittle.namespace("Dolittle",{
    assetsManager: {
        scripts: [],
        isInitialized: function() {
            return Dolittle.assetsManager.scripts.length > 0;
        },
        initialize: function () {
            var promise = Dolittle.execution.Promise.create();
            if (!Dolittle.assetsManager.isInitialized()) {
                $.get("/Dolittle/AssetsManager", { extension: "js" }, function (result) {
                    Dolittle.assetsManager.initializeFromAssets(result);
                    promise.signal();
                }, "json");
            } else {
                promise.signal();
            }
            return promise;
        },
        initializeFromAssets: function (assets) {
            if (!Dolittle.assetsManager.isInitialized()) {
                Dolittle.assetsManager.scripts = assets;
                Dolittle.namespaces.create().initialize();
            }
        },
        getScripts: function () {
            return Dolittle.assetsManager.scripts;
        },
        hasScript: function(script) {
            return Dolittle.assetsManager.scripts.some(function (scriptInSystem) {
                return scriptInSystem === script;
            });
        },
        getScriptPaths: function () {
            var paths = [];

            Dolittle.assetsManager.scripts.forEach(function (fullPath) {
                var path = Dolittle.Path.getPathWithoutFilename(fullPath);
                if (paths.indexOf(path) === -1) {
                    paths.push(path);
                }
            });
            return paths;
        }
    }
});