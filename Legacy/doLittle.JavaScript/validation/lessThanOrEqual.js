Dolittle.namespace("Dolittle.validation.ruleHandlers");
Dolittle.validation.ruleHandlers.lessThanOrEqual = {
    throwIfOptionsInvalid: function (options) {
        if (this.notSet(options)) {
            throw new Dolittle.validation.OptionsNotDefined();
        }
        if (this.notSet(options.value)) {
            var exception = new Dolittle.validation.OptionsValueNotSpecified();
            exception.message = exception.message + " 'value' is not set.";
            throw exception;
        }
    },

    throwIsValueToCheckIsNotANumber: function (value) {
        if (!Dolittle.isNumber(value)) {
            throw new Dolittle.validation.NotANumber("Value " + value + " is not a number");
        }
    },

    notSet: function (value) {
        return Dolittle.isUndefined(value) || Dolittle.isNull(value);
    },

    validate: function (value, options) {
        this.throwIfOptionsInvalid(options);
        if (this.notSet(value)) {
            return false;
        }
        this.throwIsValueToCheckIsNotANumber(value);
        return parseFloat(value) <= parseFloat(options.value);
    }
};
