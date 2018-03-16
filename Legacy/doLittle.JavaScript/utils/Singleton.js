Dolittle.namespace("Dolittle",{
    Singleton: function (typeDefinition) {
        return Dolittle.Type.extend(typeDefinition).scopeTo(window);
    }
});