Dolittle.namespace("Dolittle.markup", {
    UIElementPreparer: Dolittle.Singleton(function () {
        this.prepare = function (element, instance) {
            var result = instance.prepare(instance._type, element);
            if (result instanceof Dolittle.execution.Promise) {
                result.continueWith(function () {

                    if (!Dolittle.isNullOrUndefined(instance.template)) {
                        var UIManager = Dolittle.views.UIManager.create();

                        UIManager.handle(instance.template);

                        ko.applyBindingsToNode(instance.template, {
                            "with": instance
                        });

                        element.parentElement.replaceChild(instance.template, element);
                    }
                });
            }
        };
    })
});