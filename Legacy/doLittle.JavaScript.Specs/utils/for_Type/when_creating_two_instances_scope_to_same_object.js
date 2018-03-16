describe("when creating two instances scoped to same object", function () {
    var type = null;
    var firstInstance = null;
    var secondInstance = null;
    beforeEach(function () {
        Dolittle.dependencyResolver = {
            getDependenciesFor: sinon.stub()
        };

        type = Dolittle.Type.extend(function () {
        }).scopeTo("something");
        firstInstance = type.create();
        secondInstance = type.create();
    });

    afterEach(function () {
        Dolittle.dependencyResolver = {};
    });

    it("should return same instances", function () {
        expect(firstInstance).toBe(secondInstance);
    });
});