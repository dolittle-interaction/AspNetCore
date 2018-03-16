Dolittle.namespace("Dolittle.values", {
    Formatter: Dolittle.Type.extend(function () {
        this.supportedType = null;

        this.format = function (value, format) {
            return value;
        };
    })
});