Dolittle.namespace("Dolittle.views", {
    viewModelManager: Dolittle.Singleton(function(assetsManager, documentService, viewModelLoader, regionManager, taskFactory, viewFactory, MasterViewModel) {
        var self = this;
        this.assetsManager = assetsManager;
        this.viewModelLoader = viewModelLoader;
        this.documentService = documentService;

        this.masterViewModel = MasterViewModel;

        this.hasForView = function (viewPath) {
            var scriptFile = Dolittle.Path.changeExtension(viewPath, "js");
            scriptFile = Dolittle.Path.makeRelative(scriptFile);
            var hasViewModel = self.assetsManager.hasScript(scriptFile);
            return hasViewModel;
        };

        this.getViewModelPathForView = function (viewPath) {
            var scriptFile = Dolittle.Path.changeExtension(viewPath, "js");
            return scriptFile;
        };

        this.isLoaded = function (path) {
            var localPath = Dolittle.Path.getPathWithoutFilename(path);
            var filename = Dolittle.Path.getFilenameWithoutExtension(path);
            var namespacePath = Dolittle.namespaceMappers.mapPathToNamespace(localPath);
            if (namespacePath != null) {
                var namespace = Dolittle.namespace(namespacePath);

                if (filename in namespace) {
                    return true;
                }
            }
            return false;
        };
    })
});