Dolittle.namespace("Dolittle.mapping", {
    PropertyMap: Dolittle.Type.extend(function (sourceProperty, typeConverters) {
        var self = this;

        this.strategy = null;

        function throwIfMissingPropertyStrategy() {
            if (Dolittle.isNullOrUndefined(self.strategy)) {
                throw Dolittle.mapping.MissingPropertyStrategy.create();
            }
        }

        this.to = function (targetProperty) {
            self.strategy = function (source, target) {
                var value = ko.unwrap(source[sourceProperty]);
                var targetValue = ko.unwrap(target[targetProperty]);

                var typeAsString = null;
                if (!Dolittle.isNullOrUndefined(value)) {
                    if (!Dolittle.isNullOrUndefined(targetValue)) {
                        if (value.constructor !== targetValue.constructor) {
                            typeAsString = targetValue.constructor.name.toString();
                        }

                        if (!Dolittle.isNullOrUndefined(target[targetProperty]._typeAsString)) {
                            typeAsString = target[targetProperty]._typeAsString;
                        }
                    }

                    if (!Dolittle.isNullOrUndefined(typeAsString)) {
                        value = typeConverters.convertFrom(value.toString(), typeAsString);
                    }
                }

                if (ko.isObservable(target[targetProperty])) {
                    target[targetProperty](value);
                } else {
                    target[targetProperty] = value;
                }
            };
        };

        this.map = function (source, target) {
            throwIfMissingPropertyStrategy();

            self.strategy(source, target);
        };
    })
});