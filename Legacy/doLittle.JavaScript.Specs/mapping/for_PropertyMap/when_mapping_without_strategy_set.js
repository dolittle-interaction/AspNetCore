describe("when mapping without strategy set", function () {
    var propertyMap = Dolittle.mapping.PropertyMap.create({
        sourceProperty: "Source",
        typeConverters: {}
    });

    var missingPropertyStrategy;
    var exception = null;

    beforeEach(function () {
        missingPropertyStrategy = Dolittle.mapping.MissingPropertyStrategy;
        Dolittle.mapping.MissingPropertyStrategy = Dolittle.Type.extend(function () { });

        
        try {
            propertyMap.map({}, {});
        } catch (e) {

            exception = e;
        }
    });

    afterEach(function () {
        Dolittle.mapping.MissingPropertyStrategy = missingPropertyStrategy;
    });

    it("should throw missing property strategy", function () {
        expect(exception._type).toBe(Dolittle.mapping.MissingPropertyStrategy);
    });
});