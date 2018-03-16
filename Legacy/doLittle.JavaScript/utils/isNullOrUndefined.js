Dolittle.namespace("Dolittle",{
    isNullOrUndefined: function (value) {
        return Dolittle.isUndefined(value) || Dolittle.isNull(value);
    }
});