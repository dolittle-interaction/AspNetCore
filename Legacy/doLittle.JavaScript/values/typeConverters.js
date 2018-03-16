Dolittle.namespace("Dolittle.values", {
    typeConverters: Dolittle.Singleton(function () {
        var convertersByType = {};

        var typeConverterTypes = Dolittle.values.TypeConverter.getExtenders();
        typeConverterTypes.forEach(function (type) {
            var converter = type.create();
            convertersByType[converter.supportedType] = converter;
        });

        this.convertFrom = function (value, type) {
            var actualType = null;
            if (Dolittle.isString(type)) {
                actualType = eval(type);
            } else {
                actualType = type;
            }
            if (convertersByType.hasOwnProperty(actualType)) {
                return convertersByType[actualType].convertFrom(value);
            }

            return value;
        };

        this.convertTo = function (value) {
            if (Dolittle.isNullOrUndefined(value)) {
                return value;
            }
            for (var converter in convertersByType) {
                /* jshint eqeqeq: false */
                if (value.constructor == converter) {
                    return convertersByType[converter].convertTo(value);
                }
            }

            return value;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.typeConverters = Dolittle.values.typeConverters;
