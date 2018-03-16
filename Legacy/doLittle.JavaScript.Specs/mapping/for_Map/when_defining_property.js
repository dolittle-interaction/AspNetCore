describe("when defining property", function () {
    var propertyMapType = null;
    var propertyMapInstance = { something: 42 };
    var propertyMap = null;
    beforeEach(function () {
        propertyMapType = Dolittle.mapping.PropertyMap;
        Dolittle.mapping.PropertyMap = {
            create: sinon.stub().returns(propertyMapInstance)
        };

        var map = Dolittle.mapping.Map.create();
        propertyMap = map.property("SomeProperty");
    });


    afterEach(function () {
        Dolittle.mapping.PropertyMap = propertyMapType;
    });

    it("should create a new property map", function () {
        expect(Dolittle.mapping.PropertyMap.create.calledWith({ sourceProperty: "SomeProperty"})).toBe(true)
    });

    it("should return the created property map", function () {
        expect(propertyMap).toBe(propertyMapInstance);
    });
    
});