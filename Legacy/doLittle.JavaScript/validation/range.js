Dolittle.namespace("Dolittle.validation", {
    range: Dolittle.validation.Rule.extend(function () {
        var self = this;

        function notSet(value) {
            return Dolittle.isUndefined(value) || Dolittle.isNull(value);
        }

        function throwIfValueIsNotANumber(value, param) {
            if (!Dolittle.isNumber(value)) {
                throw new Dolittle.validation.NotANumber(param + " value " + value + " is not a number");
            }
        }


        function throwIfOptionsInvalid(options) {
            if (notSet(options)) {
                throw new Dolittle.validation.OptionsNotDefined();
            }
            if (notSet(options.max)) {
                throw new Dolittle.validation.MaxNotSpecified();
            }
            if (notSet(options.min)) {
                throw new Dolittle.validation.MinNotSpecified();
            }
            throwIfValueIsNotANumber(options.min, "min");
            throwIfValueIsNotANumber(options.max, "max");
        }


        this.validate = function (value) {
            throwIfOptionsInvalid(self.options);
            if (notSet(value)) {
                return false;
            }
            throwIfValueIsNotANumber(value, "value");
            return self.options.min <= value && value <= self.options.max;
        };

    })
});
