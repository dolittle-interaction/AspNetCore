Dolittle.namespace("Dolittle.validation", {
    required: Dolittle.validation.Rule.extend(function () {
        this.validate = function (value) {
            return !(Dolittle.isUndefined(value) || Dolittle.isNull(value) || value === "" || value === 0);
        };
    })
});

