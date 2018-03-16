Dolittle.namespace("Dolittle.validation", {
    length: Dolittle.validation.Rule.extend(function () {
        var self = this;

        function notSet(value) {
            return Dolittle.isUndefined(value) || Dolittle.isNull(value);
        }

        function throwIfValueIsNotANumber(value) {
            if (!Dolittle.isNumber(value)) {
                throw new Dolittle.validation.NotANumber("Value " + value + " is not a number");
            }
        }

        function throwIfOptionsInvalid(options) {
            if (notSet(options)) {
                throw new Dolittle.validation.OptionsNotDefined();
            }
            if (notSet(options.max)) {
                throw new Dolittle.validation.MaxLengthNotSpecified();
            }
            if (notSet(options.min)) {
                throw new Dolittle.validation.MinLengthNotSpecified();
            }
            throwIfValueIsNotANumber(options.min);
            throwIfValueIsNotANumber(options.max);
        }

        this.validate = function (value) {
            throwIfOptionsInvalid(self.options);
            if (notSet(value)) {
                value = "";
            }
            if (!Dolittle.isString(value)) {
                value = value.toString();
            }
            return self.options.min <= value.length && value.length <= self.options.max;
        };
    })
});