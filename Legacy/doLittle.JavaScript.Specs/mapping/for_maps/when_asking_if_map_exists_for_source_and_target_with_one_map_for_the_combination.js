describe("when asking if map exists for source and target with one map for the combination", function () {

    var mapType = null;
    var result = null;

    beforeEach(function () {
        mapType = Dolittle.mapping.Map;
        Dolittle.mapping.Map = Dolittle.Type.extend(function () { });
        var sourceType = Dolittle.Type.extend(function () { });
        var targetType = Dolittle.Type.extend(function () { });

        var customMap = Dolittle.mapping.Map.extend(function () {
            this.sourceType = sourceType;
            this.targetType = targetType;
        });

        var maps = Dolittle.mapping.maps.createWithoutScope();

        result = maps.hasMapFor(sourceType, targetType);
    });
    
    afterEach(function () {
        Dolittle.mapping.Map = mapType;
    });

    it("should have map", function () {
        expect(result).toBe(true);
    });
});
