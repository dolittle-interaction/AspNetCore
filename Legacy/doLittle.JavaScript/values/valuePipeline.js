Dolittle.namespace("Dolittle.values", {
    valuePipeline: Dolittle.Singleton(function (typeConverters, stringFormatter) {
        this.getValueForView = function (element, value) {
            if (Dolittle.isNullOrUndefined(value)) {
                return value;
            }
            var actualValue = ko.utils.unwrapObservable(value);
            if (Dolittle.isNullOrUndefined(actualValue)) {
                return value;
            }

            var returnValue = actualValue;

            if (stringFormatter.hasFormat(element)) {
                returnValue = stringFormatter.format(element, actualValue);
            } else {
                if (!Dolittle.isNullOrUndefined(value._typeAsString)) {
                    returnValue = typeConverters.convertTo(actualValue);
                }
            }
            return returnValue;
        };

        this.getValueForProperty = function (property, value) {
            if (Dolittle.isNullOrUndefined(property)) {
                return value;
            }
            if (Dolittle.isNullOrUndefined(value)) {
                return value;
            }
            if (!Dolittle.isNullOrUndefined(property._typeAsString)) {
                value = typeConverters.convertFrom(value, property._typeAsString);
            }

            return value;
        };
    })
});

(function () {
    var valuePipeline = Dolittle.values.valuePipeline.create();

    var oldReadValue = ko.selectExtensions.readValue;
    ko.selectExtensions.readValue = function (element) {
        var value = oldReadValue(element);

        var bindings = ko.bindingProvider.instance.getBindings(element, ko.contextFor(element));
        if (Dolittle.isNullOrUndefined(bindings)) {
            return value;
        }
        var result = valuePipeline.getValueForProperty(bindings.value, value);
        return result;
    };

    var oldWriteValue = ko.selectExtensions.writeValue;
    ko.selectExtensions.writeValue = function (element, value, allowUnset) {
        var result = value;
        var bindings = ko.bindingProvider.instance.getBindings(element, ko.contextFor(element));
        if (!Dolittle.isNullOrUndefined(bindings)) {
            result = ko.utils.unwrapObservable(valuePipeline.getValueForView(element, bindings.value));
        }
        oldWriteValue(element, result, allowUnset);
    };

    var oldSetTextContent = ko.utils.setTextContent;
    ko.utils.setTextContent = function (element, value) {
        var result = valuePipeline.getValueForView(element, value);
        oldSetTextContent(element, result);
    };

    var oldSetHtml = ko.utils.setHtml;
    ko.utils.setHtml = function (element, value) {
        var result = valuePipeline.getValueForView(element, value);
        oldSetHtml(element, result);
    };
})();