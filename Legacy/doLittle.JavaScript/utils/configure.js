Dolittle.namespace("Dolittle",{
    configureType: Dolittle.Singleton(function(assetsManager) {
        var self = this;

        var defaultUriMapper = Dolittle.StringMapper.create();
        defaultUriMapper.addMapping("{boundedContext}/{module}/{feature}/{view}", "{boundedContext}/{module}/{feature}/{view}.html");
        defaultUriMapper.addMapping("{boundedContext}/{feature}/{view}", "{boundedContext}/{feature}/{view}.html");
        defaultUriMapper.addMapping("{feature}/{view}", "{feature}/{view}.html");
        defaultUriMapper.addMapping("{view}", "{view}.html");
        Dolittle.uriMappers.default = defaultUriMapper;

        var DolittleVisualizerUriMapper = Dolittle.StringMapper.create();
        DolittleVisualizerUriMapper.addMapping("Visualizer/{module}/{view}", "/Dolittle/Visualizer/{module}/{view}.html");
        DolittleVisualizerUriMapper.addMapping("Visualizer/{view}", "/Dolittle/Visualizer/{view}.html");
        Dolittle.uriMappers.DolittleVisualizer = DolittleVisualizerUriMapper;

        this.isReady = false;
        this.readyCallbacks = [];

        this.initializeLandingPage = true;
        this.applyMasterViewModel = true;

        function onReady() {
            Dolittle.views.Region.current = document.body.region;
            self.isReady = true;
            for (var callbackIndex = 0; callbackIndex < self.readyCallbacks.length; callbackIndex++) {
                self.readyCallbacks[callbackIndex]();
            }
        }

        function hookUpNavigaionAndApplyViewModel() {
            Dolittle.navigation.navigationManager.hookup();

            if (self.applyMasterViewModel === true) {
                Dolittle.views.viewModelManager.create().masterViewModel.apply();
            }
        }

        function onStartup() {
            var configurators = Dolittle.configurator.getExtenders();
            configurators.forEach(function (configuratorType) {
                var configurator = configuratorType.create();
                configurator.config(self);
            });


            Dolittle.dependencyResolvers.DOMRootDependencyResolver.documentIsReady();
            Dolittle.views.viewModelBindingHandler.initialize();
            Dolittle.views.viewBindingHandler.initialize();
            Dolittle.navigation.navigationBindingHandler.initialize();

            if (typeof History !== "undefined" && typeof History.Adapter !== "undefined") {
                Dolittle.WellKnownTypesDependencyResolver.types.history = History;
            }

            assetsManager.initialize().continueWith(function () {
                if (self.initializeLandingPage === true) {
                    Dolittle.views.viewManager.create().initializeLandingPage().continueWith(hookUpNavigaionAndApplyViewModel);
                } else {
                    hookUpNavigaionAndApplyViewModel();
                }
                onReady();
            });
        }

        function reset() {
            self.isReady = false;
            self.readyCallbacks = [];
        }

        this.ready = function(callback) {
            if (self.isReady === true) {
                callback();
            } else {
                self.readyCallbacks.push(callback);
            }
        };

        $(function () {
            onStartup();
        });
    })
});
Dolittle.configure = Dolittle.configureType.create();
