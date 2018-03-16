Dolittle.namespace("Dolittle.validation", {
    Rule: Dolittle.Type.extend(function (options) {
        options = options || {};
        this.message = options.message || "";
        this.options = {};
        Dolittle.extend(this.options, options);

        this.validate = function (value) {
            return true;
        };
    })
});