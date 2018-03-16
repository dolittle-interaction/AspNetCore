Dolittle.namespace("Dolittle.types", {
    PropertyInfo: Dolittle.Type.extend(function (name, type) {
        this.name = name;
        this.type = type;
    })
});