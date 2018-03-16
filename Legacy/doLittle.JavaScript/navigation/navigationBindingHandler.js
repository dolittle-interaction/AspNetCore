Dolittle.namespace("Dolittle.navigation", {
    navigationBindingHandler: Dolittle.Type.extend(function () {
        function getNavigationFrameFor(valueAccessor) {
            var configurationString = ko.utils.unwrapObservable(valueAccessor());
            var configurationItems = ko.expressionRewriting.parseObjectLiteral(configurationString);
            var configuration = {};

            for (var index = 0; index < configurationItems.length; index++) {
                var item = configurationItems[index];
                configuration[item.key.trim()] = item.value.trim();
            }

            var uriMapperName = configuration.uriMapper;
            if (Dolittle.isNullOrUndefined(uriMapperName)) {
                uriMapperName = "default";
            }

            var mapper = Dolittle.uriMappers[uriMapperName];
            var frame = Dolittle.navigation.NavigationFrame.create({
                locationAware: false,
                uriMapper: mapper,
                home: configuration.home || ''
            });

            return frame;
        }

        function makeValueAccessor(navigationFrame) {
            return function () {
                return navigationFrame.currentUri();
            };
        }

        this.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var navigationFrame = getNavigationFrameFor(valueAccessor);
            navigationFrame.configureFor(element);
            return ko.bindingHandlers.view.init(element, makeValueAccessor(navigationFrame), allBindingsAccessor, viewModel, bindingContext);
        };
        this.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var navigationFrame = getNavigationFrameFor(valueAccessor);
            navigationFrame.configureFor(element);
            return ko.bindingHandlers.view.update(element, makeValueAccessor(navigationFrame), allBindingsAccessor, viewModel, bindingContext);
        };
    })
});
Dolittle.navigation.navigationBindingHandler.initialize = function () {
    ko.bindingHandlers.navigation = Dolittle.navigation.navigationBindingHandler.create();
    ko.jsonExpressionRewriting.bindingRewriteValidators.navigation = false; // Can't rewrite control flow bindings
    ko.virtualElements.allowedBindings.navigation = true;
};
