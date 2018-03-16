Dolittle.namespace("Dolittle.views", {
    ViewBindingHandlerTemplateSource: Dolittle.Type.extend(function (viewFactory) {
        var content = "";


        this.loadFor = function (element, view, region) {
            var promise = Dolittle.execution.Promise.create();

            view.load(region).continueWith(function (loadedView) {
                var wrapper = document.createElement("div");
                wrapper.innerHTML = loadedView.content;
                

                content = wrapper.innerHTML;

                if (Dolittle.isNullOrUndefined(loadedView.viewModelType)) {
                    promise.signal(loadedView);
                } else {
                    Dolittle.views.Region.current = region;
                    view.viewModelType.ensure().continueWith(function () {
                        promise.signal(loadedView);
                    });
                }
            });

            return promise;
        };

        this.data = function (key, value) { };

        this.text = function (value) {
            return content;
        };
    })
});