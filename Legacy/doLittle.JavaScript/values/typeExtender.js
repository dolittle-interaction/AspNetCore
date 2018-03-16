Dolittle.namespace("Dolittle.values", {
    typeExtender: Dolittle.Singleton(function () {
        this.extend = function (target, typeAsString) {
            target._typeAsString = typeAsString;
        };
    })
});
ko.extenders.type = Dolittle.values.typeExtender.create().extend;
