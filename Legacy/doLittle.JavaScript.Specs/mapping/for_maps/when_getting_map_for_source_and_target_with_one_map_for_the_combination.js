describe("when getting map for source and target with one map for the combination", function () {

    var mapType = null;
    var map = null;
    var customMap = null;

    beforeEach(function () {
        mapType = Dolittle.mapping.Map;
        Dolittle.mapping.Map = Dolittle.Type.extend(function () { });
        var sourceType = Dolittle.Type.extend(function () { });
        var targetType = Dolittle.Type.extend(function () { });

        customMap = Dolittle.mapping.Map.extend(function () {
            this.sourceType = sourceType;
            this.targetType = targetType;
        });

        var maps = Dolittle.mapping.maps.createWithoutScope();

        map = maps.getMapFor(sourceType, targetType);
    });
    
    afterEach(function () {
        Dolittle.mapping.Map = mapType;
    });

    it("should get the", function () {
        expect(map._type).toBe(customMap);
    });
});
