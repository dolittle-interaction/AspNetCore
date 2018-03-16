Dolittle.namespace("Dolittle.validation", {
    regex: Dolittle.validation.Rule.extend(function () {
        var self = this;

        function notSet(value) {
            return Dolittle.isUndefined(value) || Dolittle.isNull(value);
        }

        function throwIfOptionsInvalid(options) {
            if (notSet(options)) {
                throw new Dolittle.validation.OptionsNotDefined();
            }
            if (notSet(options.expression)) {
                throw new Dolittle.validation.MissingExpression();
            }
            if (!Dolittle.isString(options.expression)) {
                throw new Dolittle.validation.NotAString("Expression " + options.expression + " is not a string.");
            }
        }

        function throwIfValueIsNotString(value) {
            if (!Dolittle.isString(value)) {
                throw new Dolittle.validation.NotAString("Value " + value + " is not a string.");
            }
        }

        this.validate = function (value) {
            throwIfOptionsInvalid(self.options);
            if (notSet(value)) {
                return false;
            }
            throwIfValueIsNotString(value);
            return (value.match(self.options.expression) == null) ? false : true;
        };
    })
});


