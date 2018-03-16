Dolittle.namespace("Dolittle.types", {
    TypeInfo: Dolittle.Type.extend(function () {
        this.properties = [];
    })
});
Dolittle.types.TypeInfo.createFrom = function (instance) {
    var typeInfo = Dolittle.types.TypeInfo.create();
    var propertyInfo;
    for (var property in instance) {
        var value = instance[property];
        if (!Dolittle.isNullOrUndefined(value)) {

            var type = value.constructor;

            if (!Dolittle.isNullOrUndefined(instance[property]._type)) {
                type = instance[property]._type;
            }

            propertyInfo = Dolittle.types.PropertyInfo.create({
                name: property,
                type: type
            });
        }
        typeInfo.properties.push(propertyInfo);
    }
    return typeInfo;
};
