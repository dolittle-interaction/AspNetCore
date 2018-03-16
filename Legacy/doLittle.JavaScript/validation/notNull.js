Dolittle.namespace("Dolittle.validation", {
    notNull: Dolittle.validation.Rule.extend(function () {
        this.validate = function (value) {
            return !(Dolittle.isUndefined(value) || Dolittle.isNull(value));
        };
    })
});

