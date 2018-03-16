Dolittle.namespace("Dolittle.views", {
    viewManager: Dolittle.Singleton(function (viewFactory, pathResolvers, regionManager, UIManager, viewModelManager, viewModelLoader, viewModelTypes, documentService) {
        var self = this;


        function setViewModelForElement(element, viewModel) {
            viewModelManager.masterViewModel.setFor(element, viewModel);

            var viewModelName = documentService.getViewModelNameFor(element);

            var dataBindString = "";
            var dataBind = element.attributes.getNamedItem("data-bind");
            if (!Dolittle.isNullOrUndefined(dataBind)) {
                dataBindString = dataBind.value + ", ";
            } else {
                dataBind = document.createAttribute("data-bind");
            }
            dataBind.value = dataBindString + "viewModel: $root['" + viewModelName + "']";
            element.attributes.setNamedItem(dataBind);
        }

        this.initializeLandingPage = function () {
            var promise = Dolittle.execution.Promise.create();
            var body = document.body;
            if (body !== null) {
                var file = Dolittle.Path.getFilenameWithoutExtension(document.location.toString());
                if (file === "") {
                    file = "index";
                }

                if (pathResolvers.canResolve(body, file)) {
                    var actualPath = pathResolvers.resolve(body, file);
                    var view = viewFactory.createFrom(actualPath);
                    view.element = body;
                    view.content = body.innerHTML;
                    body.view = view;

                    var region = regionManager.getFor(view);
                    regionManager.describe(view, region).continueWith(function () {
                        if (viewModelManager.hasForView(actualPath)) {
                            var viewModelPath = viewModelManager.getViewModelPathForView(actualPath);
                            if (!viewModelManager.isLoaded(viewModelPath)) {
                                viewModelLoader.load(viewModelPath, region).continueWith(function (viewModel) {
                                    if (!Dolittle.isNullOrUndefined(viewModel)) {
                                        setViewModelForElement(body, viewModel);
                                    }
                                });
                            } else {
                                viewModelTypes.beginCreateInstanceOfViewModel(viewModelPath, region).continueWith(function (viewModel) {
                                    if (!Dolittle.isNullOrUndefined(viewModel)) {
                                        setViewModelForElement(body, viewModel);
                                    }
                                });
                            }
                        }
                    
                        UIManager.handle(body);
                        promise.signal();
                    });
                }
            }
            return promise;
        };

        this.attach = function (element) {
            UIManager.handle(element);
            viewModelManager.masterViewModel.applyTo(element);
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.viewManager = Dolittle.views.viewManager;