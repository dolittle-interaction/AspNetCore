describe("when asking if can map property with two properties and last property matches", function () {
    var propertyMapType = null;
    var result = null;
    beforeEach(function () {
        propertyMapType = Dolittle.mapping.PropertyMap;
        Dolittle.mapping.PropertyMap = {
            create: function (options) {
                return {
                };
            }
        };

        var map = Dolittle.mapping.Map.create();
        map.property("Something");
        map.property("SomeProperty");

        result = map.canMapProperty("SomeProperty");
    });


    afterEach(function () {
        Dolittle.mapping.PropertyMap = propertyMapType;
    });

    it("should be able to map property", function () {
        expect(result).toBe(true);
    });
});