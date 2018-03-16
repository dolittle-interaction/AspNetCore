describe("when asking if can map property without any property maps", function () {
    var propertyMapType = null;
    var propertyMapInstance = { something: 42 };
    var result = null;
    beforeEach(function () {
        propertyMapType = Dolittle.mapping.PropertyMap;
        Dolittle.mapping.PropertyMap = {
            create: sinon.stub().returns(propertyMapInstance)
        };

        var map = Dolittle.mapping.Map.create();
        result = map.canMapProperty("SomeProperty");
        
    });


    afterEach(function () {
        Dolittle.mapping.PropertyMap = propertyMapType;
    });

    it("should not be able to map property", function () {
        expect(result).toBe(false);
    });
});