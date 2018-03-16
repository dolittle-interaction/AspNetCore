Dolittle.namespace("Dolittle.values", {
    DateFormatter: Dolittle.values.Formatter.extend(function () {
        this.supportedType = Date;

        this.format = function (value, format) {
            return value.format(format);
        };
    })
});