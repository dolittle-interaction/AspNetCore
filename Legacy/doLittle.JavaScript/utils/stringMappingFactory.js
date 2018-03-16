Dolittle.namespace("Dolittle",{
    stringMappingFactory: Dolittle.Singleton(function () {

        this.create = function (format, mappedFormat) {
            var mapping = Dolittle.StringMapping.create({
                format: format,
                mappedFormat: mappedFormat
            });
            return mapping;
        };
    })
});