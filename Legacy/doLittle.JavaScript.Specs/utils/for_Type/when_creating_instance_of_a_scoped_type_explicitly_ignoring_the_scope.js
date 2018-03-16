describe("when creating instance of a scoped type explicitly ignoring the scope", function () {
    var existingGetDependenciesFor = null;
    var firstInstance = 41;
    var secondInstance = 42;

    beforeEach(function () {
        existingGetDependenciesFor = Dolittle.dependencyResolver.getDependenciesFor;
        Dolittle.dependencyResolver.getDependenciesFor = function () { return []; };

        var counter = 0;
        var type = Dolittle.Type.extend(function () {
            var self = this;

            this.id = counter++;
        }).scopeTo(window);

        firstInstance = type.createWithoutScope();
        secondInstance = type.createWithoutScope();
    });

    afterEach(function () {
        Dolittle.dependencyResolver.getDependenciesFor = existingGetDependenciesFor;
    });

    it("should have two different instances", function () {
        expect(firstInstance).not.toBe(secondInstance);
    });
});