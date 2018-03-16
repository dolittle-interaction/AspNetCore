Dolittle.namespace("Dolittle.validation", {
    maxLength: Dolittle.validation.Rule.extend(function() {
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
            if (notSet(options.length)) {
                throw new Dolittle.validation.MaxNotSpecified();
            }
            throwIfValueIsNotANumber(options.length);
        }


        function throwIfValueIsNotAString(string) {
            if (!Dolittle.isString(string)) {
                throw new Dolittle.validation.NotAString("Value " + string + " is not a string");
            }
        }

        this.validate = function (value) {
            throwIfOptionsInvalid(self.options);
            if (notSet(value)) {
                value = "";
            }
            throwIfValueIsNotAString(value);
            return value.length <= self.options.length;
        };
    })
});

